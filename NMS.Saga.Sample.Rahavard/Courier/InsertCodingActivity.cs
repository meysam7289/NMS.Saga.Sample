using MassTransit.Courier;
using Microsoft.Extensions.DependencyInjection;
using NMS.Saga.Sample.Contracts.Models;
using NMS.Saga.Sample.Rahavard.DataLayer;
using System;
using System.Threading.Tasks;

namespace NMS.Saga.Sample.Rahavard.Courier
{
    public class InsertCodingActivity : IActivity<Coding, Coding>
    {
        private readonly RahavardDbContext _db;
        public InsertCodingActivity(RahavardDbContext db)
        {
            _db = db;
        }
        public async Task<CompensationResult> Compensate(CompensateContext<Coding> context)
        {
            _db.Remove(context.Log);
            await _db.SaveChangesAsync();
            return context.Compensated();
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<Coding> context)
        {
            _db.Coding.Add(context.Arguments);
            await _db.SaveChangesAsync();
            return context.Completed<Coding>(context.Arguments);
        }
    }
}
