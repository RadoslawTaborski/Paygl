using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces;
using PayglService.cs;
using System.Collections.Generic;
using System.Linq;
using PayglService.cs.Models;

namespace Paygl.Models
{
    public class ViewsMemory
    {
        private static readonly List<Operation> ImportingOperations = new List<Operation>();
        private static Operation _currentOperation;
        private static int _index;

        private static List<Filter> _filters;
        public static List<Filter> Filters
        {
            get
            {
                if (_filters != null) return _filters;
                Service.LoadSettings();
                _filters = Service.Settings.Filters;
                return _filters;
            }
        }

        private static List<FiltersGroup> _filtersGroups;
        public static List<FiltersGroup> FiltersGroups
        {
            get
            {
                if (_filtersGroups != null) return _filtersGroups;
                Service.LoadSettings();
                _filtersGroups = Service.Settings.FiltersGroups;
                return _filtersGroups;
            }
        }

        public delegate void AddedGroupDelegate(OperationsGroup added);
        public static AddedGroupDelegate AddedGroup;

        public delegate void AddedParameterDelegate(IParameter added);
        public static AddedParameterDelegate AddedParameter;

        public delegate void ChangeInFiltersDelegate();
        public static ChangeInFiltersDelegate ChangeInFilters;

        public delegate void ChangeInAnalysisManagerDelegate();
        public static ChangeInAnalysisManagerDelegate ChangeInAnalysisManager;

        public delegate void EditedAnalysisViewDelegate();
        public static EditedAnalysisViewDelegate EditedAnalysisView;

        public static void AddImportingOperations(IEnumerable<Operation> operations)
        {
           ImportingOperations.AddRange(operations.ToList());
        }

        public static void InsertOperation(Operation afterThis, Operation insertThis)
        {
            var index = ImportingOperations.IndexOf(afterThis);
            ImportingOperations.Insert(index + 1,insertThis);
        }

        public static void RemoveImportingOperations(IEnumerable<Operation> operations)
        {
            ImportingOperations.Clear();
            _index = 0;
        }

        public static Operation NextOperation()
        {
            if (_index < ImportingOperations.Count - 1)
            {
                _index++;
                _currentOperation = ImportingOperations[_index];
            }

            return _currentOperation;
        }

        public static Operation PreviousOperation()
        {
            if (_index > 0)
            {
                _index--;
                _currentOperation = ImportingOperations[_index];
            }

            return _currentOperation;
        }

        public static Operation CurrentOperation()
        {
            if (ImportingOperations.Count > _index)
            {
                _currentOperation = ImportingOperations[_index];
                return _currentOperation;
            }

            return null;
        }

        internal static void RemoveImportingOperation(Operation operation)
        {
            ImportingOperations.Remove(operation);
            if (ImportingOperations.Count == 0)
            {
                _index = 0;
                _currentOperation = null;
                return;
            }
            if (_index >= ImportingOperations.Count)
            {
                _index = ImportingOperations.Count - 1;
                _currentOperation = ImportingOperations[_index];
            }

        }
    }
}
