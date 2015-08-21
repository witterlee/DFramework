using System;
using System.Collections.Generic;
using System.Linq;

namespace DFramework.RabbitCommandBus
{
    public class CommandTypeMapping
    {
        private readonly Dictionary<int, Type> _mapping;
        public CommandTypeMapping(Dictionary<int, Type> mapping)
        {
            _mapping = mapping;
        }

        public int GetTypeCode(ICommand command)
        {
            var commandType = command.GetType();

            var codeMapper = _mapping.SingleOrDefault(item => item.Value == commandType);

            if (_mapping.ContainsValue(commandType))
            {
                return codeMapper.Key;
            }
            return -3;
        }

        public Type GetTypeByCode(int commandTypeCode)
        {
            if (_mapping.ContainsKey(commandTypeCode))
            {
                return _mapping[commandTypeCode];
            }
            return null;
        }
    }
}
