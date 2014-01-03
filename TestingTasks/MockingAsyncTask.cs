using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;

namespace TestingTasks
{
    public class MockingAsyncTask
    {
        [Fact]
        public async Task can_mock_interface_methods_with_task_return()
        {
            var mock = NSubstitute.Substitute.For<ILongRunningOperation>();
            var sut = new SystemUnderTest { Operation = mock };

            await sut.Execute();
            
            var calls = mock.ReceivedCalls();

            Assert.NotEmpty(calls);
        }

        [Fact]
        public async Task can_inspect_mock_methods()
        {
            var mock = NSubstitute.Substitute.For<ILongRunningOperation>();
            var sut = new SystemUnderTest { Operation = mock };

            await sut.Execute();

            // this returns a task instead of a proxy object
            var task = mock.DoSomething();
            // so this blows up with a NotASubstituteException
            var calls = task.ReceivedCalls();

            Assert.NotEmpty(calls);
        }

        public class SystemUnderTest
        {
            public ILongRunningOperation Operation { get; set; }

            public async Task Execute()
            {
                await Operation.DoSomething();
            }
        }

        public interface ILongRunningOperation
        {
            Task DoSomething();
        }
    }
}
