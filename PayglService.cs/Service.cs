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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PayglService.cs
{
    public class Service
    {
        #region Entities
        public User User { get; private set; }
        public Language Language { get; private set; }
        public List<TransactionType> TransactionTypes { get; private set; }
        public List<TransferType> TransferTypes { get; private set; }
        public List<Frequence> Frequencies { get; private set; }
        public List<Importance> Importances { get; private set; }
        public List<Tag> Tags { get; private set; }
        public List<Operation> Operations { get; private set; }
        #endregion

        #region DbAdapters
        private DatabaseManager DbManager { get; set; }
        private DbConnector DbConnector { get; set; }
        private LanguageAdapter LanguageAdapter { get; set; }
        private UserAdapter UserAdapter { get; set; }
        private UserDetailsAdapter UserDetailsAdapter { get; set; }
        private TransactionTypeAdapter TransactionTypeAdapter { get; set; }
        private TransferTypeAdapter TransferTypeAdapter { get; set; }
        private FrequenceAdapter FrequenceAdapter { get; set; }
        private ImportanceAdapter ImportanceAdapter { get; set; }
        private TagAdapter TagAdapter { get; set; }
        private OperationAdapter OperationAdapter { get; set; }
        private OperationDetailsAdapter OperationDetailsAdapter { get; set; }
        private OperationTagAdapter OperationTagRelationAdapter { get; set; }
        #endregion

        #region Mappers
        private LanguageMapper LanguageMapper { get; set; }
        private UserMapper UserMapper { get; set; }
        private UserDetailsMapper UserDetailsMapper { get; set; }
        private TransactionTypeMapper TransactionTypeMapper { get; set; }
        private TransferTypeMapper TransferTypeMapper { get; set; }
        private FrequenceMapper FrequenceMapper { get; set; }
        private ImportanceMapper ImportanceMapper { get; set; }
        private TagMapper TagMapper { get; set; }
        private OperationMapper OperationMapper { get; set; }
        private OperationDetailsMapper OperationDetailsMapper { get; set; }
        private RelationMapper RelationMapper { get; set; }
        #endregion

        public delegate IDalEntity Mapper(IEntity entity, int id);

        public Service()
        {
            DbManager = new DatabaseManager(new MySqlConnectionFactory(), "localhost", "paygl", "root", "");
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
        }

        public void LoadAttributes()
        {
            var languages = LanguageMapper.ConvertToBusinessLogicEntitiesCollection(LanguageAdapter.GetAll());
            Language = languages.Where(l => l.ShortName == "pl-PL").First();

            var dalUser = UserAdapter.GetById(1);
            User = UserMapper.ConvertToBusinessLogicEntity(dalUser);
            User.SetDetails(UserDetailsMapper.ConvertToBusinessLogicEntity(UserDetailsAdapter.GetById(dalUser.DetailsId)));

            TransactionTypes = TransactionTypeMapper.ConvertToBusinessLogicEntitiesCollection(TransactionTypeAdapter.GetAll($"language_id={Language.Id}")).ToList();
            TransferTypes = TransferTypeMapper.ConvertToBusinessLogicEntitiesCollection(TransferTypeAdapter.GetAll($"language_id={Language.Id}")).ToList();
            Frequencies = FrequenceMapper.ConvertToBusinessLogicEntitiesCollection(FrequenceAdapter.GetAll($"language_id={Language.Id}")).ToList();
            Importances = ImportanceMapper.ConvertToBusinessLogicEntitiesCollection(ImportanceAdapter.GetAll($"language_id={Language.Id}")).ToList();
            Tags = TagMapper.ConvertToBusinessLogicEntitiesCollection(TagAdapter.GetAll($"language_id={Language.Id}")).ToList();
        }

        public void LoadOperations()
        {
            OperationMapper.Update(User, Importances, Frequencies, TransactionTypes, TransferTypes);
            Operations = OperationMapper.ConvertToBusinessLogicEntitiesCollection(OperationAdapter.GetAll($"user_id={User.Id}")).ToList();

            if (Operations.Count > 0)
            {
                var filter = "";
                foreach (var operation in Operations)
                {
                    filter += $"operation_id={operation.Id} AND ";
                }
                filter = filter.Substring(0, filter.Length - 4);

                RelationMapper.Update(Operations, Tags);
                var relations = RelationMapper.ConvertToBusinessLogicEntitiesCollection(OperationTagRelationAdapter.GetAll(filter));
                var relTags = relations.Item1;
                var relOperations = relations.Item2;

                foreach (var operation in Operations)
                {
                    operation.SetDetailsList(OperationDetailsMapper.ConvertToBusinessLogicEntitiesCollection(OperationDetailsAdapter.GetAll($"operation_id={operation.Id}")));
                    operation.SetTags(relTags.Where(r => r.OperationId == operation.Id));
                }

                foreach (var tag in Tags)
                {
                    tag.SetOperations(relOperations.Where(r => r.TagId == tag.Id));
                }
            }
        }

        public List<Operation> Import()
        {
            var ignored = new List<string> {
                " TABORSKI RADOSŁAW, MASŁOWICE 10, 98-300 MASŁOWICE "
            };

            var importFactory = ImportFactory.GetFactory("ING");
            IImporter importer = importFactory.CreateImporter();

            var transactions = importer.ReadTransactions();
            foreach (var ignoredItem in ignored)
            {
                transactions = transactions.Where(t => !Regex.Match(t.ContractorData, ignoredItem).Success);
            }

            return new TransactionToOperationMapper().ConvertToEntitiesCollection(transactions, User, TransactionTypes, TransferTypes).ToList();

            //operations[0].SetFrequence(Frequencies[4]);
            //operations[0].SetImportance(Importances[2]);
            //operations[0].AddTag(Tags[4]);
            //operations[0].IsDirty = true;

            //UpdateOperationComplex(operations[0]);
        }

        public void UpdateOperationComplex(Operation operation)
        {
            if (operation.Parent != null)
            {
                UpdateOperationComplex(operation);
            }

            if (operation.DetailsList != null)
            {
                foreach (var item in operation.DetailsList)
                {
                    UpdateOperationDetails(item, operation.Id.Value);
                }
            }

            UpdateOperation(operation);

            foreach (var tag in operation.Tags)
            {
                InsertRelation(tag, operation);
            }
        }

        #region private

        #region Updates

        private int UpdateOperationDetails(OperationDetails details, int operationId)
        {
            return UpdateBusinessEntity(details, operationId, OperationDetailsAdapter, OperationDetailsMapper.ConvertToDALEntity);
        }

        private int UpdateOperation(Operation operation)
        {
            return UpdateBusinessEntity(operation, OperationAdapter, OperationMapper.ConvertToDALEntity);
        }

        private int UpdateUser(User user)
        {
            return UpdateBusinessEntity(user, UserAdapter, UserMapper.ConvertToDALEntity);
        }

        private int UpdateUserDetails(UserDetails userDetails)
        {
            return UpdateBusinessEntity(userDetails, UserDetailsAdapter, UserDetailsMapper.ConvertToDALEntity);
        }

        private int UpdateImportance(Importance importance)
        {
            return UpdateBusinessEntity(importance, ImportanceAdapter, ImportanceMapper.ConvertToDALEntity);
        }

        private int UpdateFrequence(Frequence frequence)
        {
            return UpdateBusinessEntity(frequence, FrequenceAdapter, FrequenceMapper.ConvertToDALEntity);
        }

        private int UpdateTransactionType(TransactionType transactionType)
        {
            return UpdateBusinessEntity(transactionType, TransactionTypeAdapter, TransactionTypeMapper.ConvertToDALEntity);
        }

        private int UpdateTransferType(TransferType transferType)
        {
            return UpdateBusinessEntity(transferType, TransferTypeAdapter, TransferTypeMapper.ConvertToDALEntity);
        }

        private int UpdateTag(Tag tag)
        {
            return UpdateBusinessEntity(tag, TagAdapter, TagMapper.ConvertToDALEntity);
        }

        private int InsertRelation(RelTag tag, Operation operation)
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

        private void DeleteRelation(RelTag tag, Operation operation)
        {
            operation.RemoveTag(tag);
            OperationTagRelationAdapter.Delete(RelationMapper.ConvertToDALEntity(tag, operation));
        }

        #endregion

        #region UpdateBusinessEntity
        private int UpdateBusinessEntity<Type, DalType>(Type entity, int id, IAdapter<DalType> adapter, Func<Type, int, DalType> convertToDALEntity)
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

        private int UpdateBusinessEntity<Type, DalType>(Type entity, IAdapter<DalType> adapter, Func<Type, DalType> convertToDALEntity)
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
