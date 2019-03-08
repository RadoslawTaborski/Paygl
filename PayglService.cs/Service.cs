using Analyzer;
using DataAccess;
using DataBaseWithBusinessLogicConnector;
using DataBaseWithBusinessLogicConnector.Dal.Adapters;
using DataBaseWithBusinessLogicConnector.Dal.DalEntities;
using DataBaseWithBusinessLogicConnector.Dal.Mappers;
using DataBaseWithBusinessLogicConnector.Entities;
using DataBaseWithBusinessLogicConnector.Interfaces;
using DataBaseWithBusinessLogicConnector.Interfaces.Dal;
using Importer;
using PayglService.cs.Helpers;
using PayglService.Helpers.Serializers;
using PayglService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PayglService.cs
{
    public static class Service
    {
        public static Settings2 settings;

        #region Entities
        public static User User { get; private set; }
        public static Language Language { get; private set; }
        public static List<TransactionType> TransactionTypes { get; private set; }
        public static List<TransferType> TransferTypes { get; private set; }
        public static List<Frequence> Frequencies { get; private set; }
        public static List<Importance> Importances { get; private set; }
        public static List<Tag> Tags { get; private set; }
        public static List<Operation> Operations { get; private set; }
        public static List<OperationsGroup> OperationsGroups { get; private set; }
        #endregion

        #region DbAdapters
        private static DatabaseManager DbManager { get; set; }
        private static DbConnector DbConnector { get; set; }
        private static LanguageAdapter LanguageAdapter { get; set; }
        private static UserAdapter UserAdapter { get; set; }
        private static UserDetailsAdapter UserDetailsAdapter { get; set; }
        private static TransactionTypeAdapter TransactionTypeAdapter { get; set; }
        private static TransferTypeAdapter TransferTypeAdapter { get; set; }
        private static FrequenceAdapter FrequenceAdapter { get; set; }
        private static ImportanceAdapter ImportanceAdapter { get; set; }
        private static TagAdapter TagAdapter { get; set; }
        private static OperationAdapter OperationAdapter { get; set; }
        private static OperationDetailsAdapter OperationDetailsAdapter { get; set; }
        private static OperationTagAdapter OperationTagRelationAdapter { get; set; }
        private static OperationsGroupAdapter OperationsGroupAdapter { get; set; }
        private static OperationsGroupTagAdapter OperationsGroupRelationAdapter { get; set; }
        #endregion

        #region Mappers
        private static LanguageMapper LanguageMapper { get; set; }
        private static UserMapper UserMapper { get; set; }
        private static UserDetailsMapper UserDetailsMapper { get; set; }
        private static TransactionTypeMapper TransactionTypeMapper { get; set; }
        private static TransferTypeMapper TransferTypeMapper { get; set; }
        private static FrequenceMapper FrequenceMapper { get; set; }
        private static ImportanceMapper ImportanceMapper { get; set; }
        private static TagMapper TagMapper { get; set; }
        private static OperationMapper OperationMapper { get; set; }
        private static OperationDetailsMapper OperationDetailsMapper { get; set; }
        private static RelationMapper RelationMapper { get; set; }
        private static OperationsGroupMapper OperationsGroupMapper { get; set; }
        private static OperationsGroupRelationMapper OperationsGroupRelationMapper { get; set; }
        #endregion

        public delegate IDalEntity Mapper(IEntity entity, int id);

        public static void SetService()
        {
            settings = new Settings2();

            var dataBaseData = ConfigurationManager.DataBaseData();
            DbManager = new DatabaseManager(new MySqlConnectionFactory(), dataBaseData.Address, dataBaseData.Port, dataBaseData.Table, dataBaseData.Login, dataBaseData.Password);
            DbConnector = new DbConnector(DbManager);

            LanguageAdapter = new LanguageAdapter(DbConnector);
            UserAdapter = new UserAdapter(DbConnector);
            UserDetailsAdapter = new UserDetailsAdapter(DbConnector);
            TransactionTypeAdapter = new TransactionTypeAdapter(DbConnector);
            TransferTypeAdapter = new TransferTypeAdapter(DbConnector);
            FrequenceAdapter = new FrequenceAdapter(DbConnector);
            ImportanceAdapter = new ImportanceAdapter(DbConnector);
            TagAdapter = new TagAdapter(DbConnector);
            OperationAdapter = new OperationAdapter(DbConnector);
            OperationDetailsAdapter = new OperationDetailsAdapter(DbConnector); 
            OperationTagRelationAdapter = new OperationTagAdapter(DbConnector);
            OperationsGroupAdapter = new OperationsGroupAdapter(DbConnector);
            OperationsGroupRelationAdapter = new OperationsGroupTagAdapter(DbConnector);

            LanguageMapper = new LanguageMapper();
            UserMapper = new UserMapper();
            UserDetailsMapper = new UserDetailsMapper();
            TransactionTypeMapper = new TransactionTypeMapper();
            TransferTypeMapper = new TransferTypeMapper();
            FrequenceMapper = new FrequenceMapper();
            ImportanceMapper = new ImportanceMapper();
            TagMapper = new TagMapper();
            OperationMapper = new OperationMapper();
            OperationDetailsMapper = new OperationDetailsMapper();
            RelationMapper = new RelationMapper();
            OperationsGroupMapper = new OperationsGroupMapper();
            OperationsGroupRelationMapper = new OperationsGroupRelationMapper();

            SetMainConfigurations();
        }

        private static void SetMainConfigurations()
        {
            var languages = LanguageMapper.ConvertToBusinessLogicEntitiesCollection(LanguageAdapter.GetAll());
            Language = languages.Where(l => l.ShortName == ConfigurationManager.Language()).First();

            DalUser dalUser;
            var dalUsers = UserAdapter.GetAll($"login='{ConfigurationManager.User().Login}'");
            if (dalUsers.Count() == 0)
            {
                throw new ArgumentException("User not exist");
            }
            else
            {
                if (dalUsers.ElementAt(0).Password == ConfigurationManager.User().Password)
                {
                    dalUser = dalUsers.ElementAt(0);
                }
                else
                {
                    throw new ArgumentException("Bad password");
                }
            }
            User = UserMapper.ConvertToBusinessLogicEntity(dalUser);
            User.SetDetails(UserDetailsMapper.ConvertToBusinessLogicEntity(UserDetailsAdapter.GetById(dalUser.DetailsId)));
        }

        public static void LoadAttributes()
        {
            TransactionTypes = TransactionTypeMapper.ConvertToBusinessLogicEntitiesCollection(TransactionTypeAdapter.GetAll($"language_id={Language.Id}")).ToList();
            TransferTypes = TransferTypeMapper.ConvertToBusinessLogicEntitiesCollection(TransferTypeAdapter.GetAll($"language_id={Language.Id}")).ToList();
            Frequencies = FrequenceMapper.ConvertToBusinessLogicEntitiesCollection(FrequenceAdapter.GetAll($"language_id={Language.Id}")).ToList();
            Importances = ImportanceMapper.ConvertToBusinessLogicEntitiesCollection(ImportanceAdapter.GetAll($"language_id={Language.Id}")).ToList();
            Tags = TagMapper.ConvertToBusinessLogicEntitiesCollection(TagAdapter.GetAll($"language_id={Language.Id}")).ToList();
        }

        public static void LoadOperations()
        {
            OperationMapper.Update(User, OperationsGroups, Importances, Frequencies, TransactionTypes, TransferTypes);
            Operations = OperationMapper.ConvertToBusinessLogicEntitiesCollection(OperationAdapter.GetAll($"user_id={User.Id}")).ToList();

            if (Operations.Count > 0)
            {
                var filter = "";
                foreach (var operation in Operations)
                {
                    filter += $"operation_id={operation.Id} OR ";
                }
                filter = filter.Substring(0, filter.Length - 4);

                RelationMapper.Update(Operations, Tags);
                var relations = RelationMapper.ConvertToBusinessLogicEntitiesCollection(OperationTagRelationAdapter.GetAll(filter));
                var relTags = relations.Item1;
                var relOperations = relations.Item2;

                foreach (var operation in Operations)
                {
                    operation.SetDetailsList(OperationDetailsMapper.ConvertToBusinessLogicEntitiesCollection(OperationDetailsAdapter.GetAll($"operation_id={operation.Id}")));
                    operation.SetTags(relTags.Where(r => r.RelatedId == operation.Id));
                }

                foreach (var tag in Tags)
                {
                    tag.SetOperations(relOperations.Where(r => r.TagId == tag.Id));
                }
            }
        }

        public static void LoadOperationsGroups()
        {
            OperationsGroupMapper.Update(User, Importances, Frequencies);
            OperationsGroups = OperationsGroupMapper.ConvertToBusinessLogicEntitiesCollection(OperationsGroupAdapter.GetAll($"user_id={User.Id}")).ToList();

            if (OperationsGroups.Count > 0)
            {
                var filter = "";
                foreach (var group in OperationsGroups)
                {
                    filter += $"operation_group_id={group.Id} OR ";
                }
                filter = filter.Substring(0, filter.Length - 4);

                OperationsGroupRelationMapper.Update(Tags);
                var relations = OperationsGroupRelationMapper.ConvertToBusinessLogicEntitiesCollection(OperationsGroupRelationAdapter.GetAll(filter));

                foreach (var group in OperationsGroups)
                {
                    group.SetTags(relations.Where(r => r.RelatedId == group.Id));
                }
            }
        }

        public static List<Operation> Import()
        {
            var ignored = ConfigurationManager.IgnoredTransaction();

            var importFactory = ImportFactory.GetFactory(ConfigurationManager.BankName());
            IImporter importer = importFactory.CreateImporter();
            var transactions = new List<Transaction>();
            var path = ConfigurationManager.PathToImportFiles();

            foreach (string file in Directory.EnumerateFiles(path, "*.csv"))
            {
                transactions.AddRange(importer.ReadTransactions(file));
                File.Move(file, file + ".taken");

                foreach (var ignoredItem in ignored)
                {
                    if (ignoredItem.DescriptionRegex == "")
                    {
                        ignoredItem.DescriptionRegex = ".*";
                    }

                    if (ignoredItem.TitleRegex == "")
                    {
                        ignoredItem.TitleRegex = ".*";
                    }

                    transactions = transactions.Where(t => !Regex.Match(t.ContractorData, ignoredItem.DescriptionRegex).Success || !Regex.Match(t.Title, ignoredItem.TitleRegex).Success).ToList();
                    transactions.Reverse();
                }
            }

            return new TransactionToOperationMapper().ConvertToEntitiesCollection(transactions, User, Importances, Frequencies, Tags, TransactionTypes, TransferTypes).ToList();
        }

        public static void UpdateOperationsGroupComplex(OperationsGroup group)
        {
            UpdateOperationGroup(group);

            for (int i = group.Tags.Count - 1; i > -1; i--)
            {
                RelTag tag = group.Tags[i];
                if (tag.IsMarkForDeletion && !tag.IsDirty)
                {
                    DeleteRelation(tag, group);
                    continue;
                }
                if (tag.IsDirty)
                {
                    InsertRelation(tag, group);
                }
            }

            foreach(var operation in group.Operations)
            {
                operation.SetFrequence(group.Frequence);
                operation.SetImportance(group.Importance);
                UpdateOperationTags(operation, group.Tags);
                UpdateOperationComplex(operation);
            }
        }

        private static void UpdateOperationTags(Operation operation, List<RelTag> parentTag)
        {
            operation.Tags.ForEach(t => t.IsMarkForDeletion = true);
            foreach (var relTag in parentTag)
            {
                operation.AddTag(relTag.Tag);
            }
        }

        private static int InsertRelation(RelTag tag, OperationsGroup group)
        {
            if (tag.IsDirty)
            {
                var newId = OperationsGroupRelationAdapter.Insert(OperationsGroupRelationMapper.ConvertToDALEntity(tag, group));
                tag.UpdateId(newId);
                tag.IsDirty = false;
            }

            return tag.Id.Value;
        }

        private static void DeleteRelation(RelTag tag, OperationsGroup group)
        {
            group.RemoveTag(tag);
            OperationsGroupRelationAdapter.Delete(OperationsGroupRelationMapper.ConvertToDALEntity(tag, group));
        }

        public static void UpdateOperationComplex(Operation operation)
        {
            if (operation.DetailsList != null)
            {
                foreach (var item in operation.DetailsList)
                {
                    UpdateOperationDetails(item, operation.Id.Value);
                }
            }

            UpdateOperation(operation);

            for (int i = operation.Tags.Count-1; i > -1 ; i--)
            {
                RelTag tag = operation.Tags[i];
                if (tag.IsMarkForDeletion && !tag.IsDirty)
                {
                    DeleteRelation(tag, operation);
                    continue;
                }
                if (tag.IsDirty)
                {
                    InsertRelation(tag, operation);
                }
            }
        }

        public static void LoadSettings()
        {
            var path = @"D:\Programowanie\C#\Paygl\Queries\settings";
            if (File.Exists(path))
            {
                settings = BinarySerializer<Settings2>.Deserialize(path);
            }
        }

        public static void SaveSettings()
        {
            var path = @"D:\Programowanie\C#\Paygl\Queries\settings";
            BinarySerializer<Settings2>.Serialize(path, settings);
        }

        public static void SetSettings(List<FiltersGroup> filtersGroups)
        {
            settings.FiltersGroups = filtersGroups;
        }

        public static void SetSettings(List<Filter> filters)
        {
            settings.Filters = filters;
        }

        public static List<KeyValuePair<string,QueryNode>> ReadQuery()
        {
            var result = new List<KeyValuePair<string, QueryNode>>();
            string line;
            var file = new StreamReader(@"D:\Programowanie\C#\Paygl\Queries\queries.txt"); //TODO: to file
            while ((line = file.ReadLine()) != null)
            {
                var substrings = line.Split(':');
                if (substrings.Count() != 2)
                {
                    throw new Exception("File with queries has wrong data");
                }
                if (substrings[1].IndexOf("TransferType") == -1)
                {
                    result.Add(new KeyValuePair<string, QueryNode>(substrings[0], Analyzer.Analyzer.StringToQuery(substrings[1])));
                } else
                {
                    result.Add(new KeyValuePair<string, QueryNode>(substrings[0], Analyzer.Analyzer.StringToQuery(substrings[1], true)));
                }
            }

            return result;
        }

        public static void SaveFilter(string name, string query)
        {
            var added = false;
            string sourceFile = @"D:\Programowanie\C#\Paygl\Queries\queries.txt";
            string destinationFile = @"D:\Programowanie\C#\Paygl\Queries\queries.txt";

            // Read the old file.
            string[] lines = File.ReadAllLines(sourceFile);

            // Write the new file over the old file.
            using (StreamWriter writer = new StreamWriter(destinationFile))
            {
                for (int currentLine = 1; currentLine <= lines.Length; ++currentLine)
                {
                    var line = lines[currentLine - 1];
                    if (line.Split(':')[0] == name)
                    {
                        line = $"{name}: {query}";
                        added = true;
                    }
                    writer.WriteLine(line);
                }
                if (!added)
                {
                    writer.WriteLine($"{name}: {query}");
                }
            }
        }

        public static void RemoveFilter(string name)
        {
            string sourceFile = @"D:\Programowanie\C#\Paygl\Queries\queries.txt";
            string destinationFile = @"D:\Programowanie\C#\Paygl\Queries\queries.txt";

            // Read the old file.
            string[] lines = File.ReadAllLines(sourceFile);

            // Write the new file over the old file.
            using (StreamWriter writer = new StreamWriter(destinationFile))
            {
                for (int currentLine = 1; currentLine <= lines.Length; ++currentLine)
                {
                    var line = lines[currentLine - 1];
                    if (line.Split(':')[0] == name)
                    {
                        continue;
                    }
                    writer.WriteLine(line);
                }
            }
        }

        #region private
        #region Updates

        private static int UpdateOperationDetails(OperationDetails details, int operationId)
        {
            return UpdateBusinessEntity(details, operationId, OperationDetailsAdapter, OperationDetailsMapper.ConvertToDALEntity);
        }

        private static int UpdateOperation(Operation operation)
        {
            return UpdateBusinessEntity(operation, OperationAdapter, OperationMapper.ConvertToDALEntity);
        }

        private static int UpdateOperationGroup(OperationsGroup group)
        {
            return UpdateBusinessEntity(group, OperationsGroupAdapter, OperationsGroupMapper.ConvertToDALEntity);
        }

        private static int UpdateUser(User user)
        {
            return UpdateBusinessEntity(user, UserAdapter, UserMapper.ConvertToDALEntity);
        }

        private static int UpdateUserDetails(UserDetails userDetails)
        {
            return UpdateBusinessEntity(userDetails, UserDetailsAdapter, UserDetailsMapper.ConvertToDALEntity);
        }

        private static int UpdateImportance(Importance importance)
        {
            return UpdateBusinessEntity(importance, ImportanceAdapter, ImportanceMapper.ConvertToDALEntity);
        }

        private static int UpdateFrequence(Frequence frequence)
        {
            return UpdateBusinessEntity(frequence, FrequenceAdapter, FrequenceMapper.ConvertToDALEntity);
        }

        private static int UpdateTransactionType(TransactionType transactionType)
        {
            return UpdateBusinessEntity(transactionType, TransactionTypeAdapter, TransactionTypeMapper.ConvertToDALEntity);
        }

        private static int UpdateTransferType(TransferType transferType)
        {
            return UpdateBusinessEntity(transferType, TransferTypeAdapter, TransferTypeMapper.ConvertToDALEntity);
        }

        private static int UpdateTag(Tag tag)
        {
            return UpdateBusinessEntity(tag, TagAdapter, TagMapper.ConvertToDALEntity);
        }

        private static int InsertRelation(RelTag tag, Operation operation)
        {
            if (tag.IsDirty)
            {
                var newId = OperationTagRelationAdapter.Insert(RelationMapper.ConvertToDALEntity(tag, operation));
                tag.UpdateId(newId);
                tag.IsDirty = false;
                var relOperation = tag.Tag.Operations.Where(o => o.Operation == operation).First();
                relOperation.UpdateId(newId);
                relOperation.IsDirty = false;
            }

            return tag.Id.Value;
        }

        private static void DeleteRelation(RelTag tag, Operation operation)
        {
            operation.RemoveTag(tag);
            OperationTagRelationAdapter.Delete(RelationMapper.ConvertToDALEntity(tag, operation));
        }

        #endregion

        #region UpdateBusinessEntity
        private static int UpdateBusinessEntity<Type, DalType>(Type entity, int id, IAdapter<DalType> adapter, Func<Type, int, DalType> convertToDALEntity)
        where Type : IEntity where DalType : IDalEntity
        {
            if (entity.IsDirty)
            {
                if (entity.Id == null)
                {
                    var newId = adapter.Insert(convertToDALEntity(entity, id));
                    entity.UpdateId(newId);
                }
                else
                {
                    adapter.Update(convertToDALEntity(entity, id));
                }
                entity.IsDirty = false;
            }
            return entity.Id.Value;
        }

        private static int UpdateBusinessEntity<Type, DalType>(Type entity, IAdapter<DalType> adapter, Func<Type, DalType> convertToDALEntity)
        where Type : IEntity where DalType : IDalEntity
        {
            if (entity.IsDirty)
            {
                if (entity.Id == null)
                {
                    var newId = adapter.Insert(convertToDALEntity(entity));
                    entity.UpdateId(newId);
                }
                else
                {
                    adapter.Update(convertToDALEntity(entity));
                }
                entity.IsDirty = false;
            }
            return entity.Id.Value;
        }
        #endregion
        #endregion
    }
}
