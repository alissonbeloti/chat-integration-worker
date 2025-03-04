using Core.Domain.Entities;

using Infrastructure.Factories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Integration
{
    public class InstagramClent : IInstagramClient
    {
        public Task<IEnumerable<InstagramDirectMessage>> GetDirectMessages(string conversationId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Login()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Logout()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendDirectMessage(string recipient, string message)
        {
            throw new NotImplementedException();
        }
    }
}
