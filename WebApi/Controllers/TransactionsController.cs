using System;
using System.Collections.Generic;
using System.Linq;
using AnaDron.SDTest.Model;
using AnaDron.SDTest.WebApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AnaDron.SDTest.WebApi.Controllers {
	[Route("api/transactions")]
	public sealed class TransactionsController:AuthorizeController {
		readonly IUnitOfWork _unitOfWork;
		readonly IUserService _userService;
		readonly ITransactionService _transactionService;

		public TransactionsController(IUnitOfWork unitOfWork, IUserService userService, ITransactionService transactionService) {
			_unitOfWork = unitOfWork;
			_userService = userService;
			_transactionService = transactionService;
		}

		[HttpGet]
		public ActionResult GetLast([FromQuery] TransactionsGetLastRequest request) {
			var userId = UserId;
			var limit = request.Limit ?? 10;
			List<TransactionModel> transactions;
			try {
				transactions = _transactionService.GetLast(userId, limit).ToList();
			} catch (UserNotFoundException) {
				return Unauthorized();
			}

			return Ok(new {
				transactions = transactions.Select(x => new {
					dt = x.DT,
					direction = userId == x.Payee.Id ? "Outside" : "Inside",
					contragent = userId == x.Payee.Id ? new { Id = x.Recepient.Id, Name = x.Recepient.Name } : new { Id = x.Payee.Id, Name = x.Payee.Name },
					amount = x.Amount
				})
			});
		}

		[HttpPost]
		public ActionResult Add(TransactionsAddRequest request) {
			var userId = UserId;
			if (request.ContragentId == userId) return BadRequest(new { message = "Контрагент является текущим пользователем" });
			try {
				_transactionService.Add(userId, request.ContragentId, request.Amount);
			} catch (UserNotFoundException e) {
				switch (e.Name) {
				case "Payee": return Unauthorized();
				case "Recepient": return BadRequest(new { message = "Контрагент не найден" });
				}
			} catch (ArgumentException e) {
				switch (e.ParamName) {
				case "amount":
					return BadRequest(new { message = "Недостаточно средств" });
				}
				throw;
			}

			_unitOfWork.Complete();

			return Ok();
		}
	}
}
