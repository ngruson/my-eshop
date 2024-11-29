using Ardalis.Result;
using MediatR;

namespace eShop.Invoicing.API.Application.Commands.CreateInvoice;

internal record CreateInvoiceCommand(Guid OrderId) : IRequest<Result>;
