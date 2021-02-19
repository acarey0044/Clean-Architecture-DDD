using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Clean.Architecture.Ddd.Application
{
    public class TestMediatrCommand : IRequest<bool>
    {
    }

    public class TestMediatrHandler : IRequestHandler<TestMediatrCommand, bool>
    {
        public Task<bool> Handle(TestMediatrCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }
}
