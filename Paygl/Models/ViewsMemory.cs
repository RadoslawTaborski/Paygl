using DataBaseWithBusinessLogicConnector.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paygl.Models
{
    public class ViewsMemory
    {
        private static List<Operation> _importingOperations = new List<Operation>();
        private static Operation _currentOperation;
        private static int _index = 0;


        public static void AddImportingOperations(IEnumerable<Operation> operations)
        {
           _importingOperations.AddRange(operations.ToList());
        }

        public static void InsertOperation(Operation afterThis, Operation insertThis)
        {
            var index = _importingOperations.IndexOf(afterThis);
            _importingOperations.Insert(index + 1,insertThis);
        }

        public static void RemoveImportingOperations(IEnumerable<Operation> operations)
        {
            _importingOperations.Clear();
            _index = 0;
        }

        public static Operation NextOperation()
        {
            if (_index < _importingOperations.Count - 1)
            {
                _index++;
                _currentOperation = _importingOperations[_index];
            }

            return _currentOperation;
        }

        public static Operation PreviousOperation()
        {
            if (_index > 0)
            {
                _index--;
                _currentOperation = _importingOperations[_index];
            }

            return _currentOperation;
        }

        public static Operation CurrentOperation()
        {
            if (_importingOperations.Count > _index)
            {
                _currentOperation = _importingOperations[_index];
                return _currentOperation;
            }

            return null;
        }

        internal static void RemoveImportingOperation(Operation operation)
        {
            _importingOperations.Remove(operation);
            if (_importingOperations.Count == 0)
            {
                _index = 0;
                _currentOperation = null;
                return;
            }
            if (_index >= _importingOperations.Count)
            {
                _index = _importingOperations.Count - 1;
                _currentOperation = _importingOperations[_index];
            }

        }
    }
}
