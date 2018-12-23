using DataAccess;
using DataBaseWithBusinessLogicConnector;
using DataBaseWithBusinessLogicConnector.Dal.Adapters;
using DataBaseWithBusinessLogicConnector.Dal.Mappers;
using DataBaseWithBusinessLogicConnector.Entities;
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
        public User User { get; private set; }
        public Language Language { get; private set; }
        public List<TransactionType> TransactionTypes { get; private set; }
        public List<TransferType> TransferTypes { get; private set; }
        public List<Frequence> Frequencies { get; private set; }
        public List<Importance> Importances { get; private set; }
        public List<Tag> Tags { get; private set; }
        public List<Operation> Operations { get; private set; }


        public DatabaseManager DbManager { get; private set; }
        public DbConnector DbConnector { get; private set; }
        public LanguageAdapter LanguageAdapter { get; private set; }
        public UserAdapter UserAdapter { get; private set; }
        public UserDetailsAdapter UserDetailsAdapter { get; private set; }
        public TransactionTypeAdapter TransactionTypeAdapter { get; private set; }
        public TransferTypeAdapter TransferTypeAdapter { get; private set; }
        public FrequenceAdapter FrequenceAdapter { get; private set; }
        public ImportanceAdapter ImportanceAdapter { get; private set; }
        public TagAdapter TagAdapter { get; private set; }
        public OperationAdapter OperationAdapter { get; private set; }
        public OperationDetailsAdapter OperationDetailsAdapter { get; private set; }
        public OperationTagAdapter OperationTagRelationAdapter { get; private set; }

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
        }

        public void LoadAttributes()
        {
            var languages = new LanguageMapper().ConvertToBusinessLogicEntitiesCollection(LanguageAdapter.GetAll());
            Language = languages.Where(l => l.ShortName == "pl-PL").First();

            var dalUser = UserAdapter.GetById(1);
            User = new UserMapper().ConvertToBusinessLogicEntity(dalUser);
            User.SetDetails(new UserDetailsMapper().ConvertToBusinessLogicEntity(UserDetailsAdapter.GetById(dalUser.DetailsId)));

            TransactionTypes = new TransactionTypeMapper().ConvertToBusinessLogicEntitiesCollection(TransactionTypeAdapter.GetAll($"language_id={Language.Id}")).ToList();
            TransferTypes = new TransferTypeMapper().ConvertToBusinessLogicEntitiesCollection(TransferTypeAdapter.GetAll($"language_id={Language.Id}")).ToList();
            Frequencies = new FrequenceMapper().ConvertToBusinessLogicEntitiesCollection(FrequenceAdapter.GetAll($"language_id={Language.Id}")).ToList();
            Importances = new ImportanceMapper().ConvertToBusinessLogicEntitiesCollection(ImportanceAdapter.GetAll($"language_id={Language.Id}")).ToList();
            Tags = new TagMapper().ConvertToBusinessLogicEntitiesCollection(TagAdapter.GetAll($"language_id={Language.Id}")).ToList();
        }

        public void LoadOperations()
        {
            Operations = new OperationMapper(User, Importances, Frequencies, TransactionTypes, TransferTypes).ConvertToBusinessLogicEntitiesCollection(OperationAdapter.GetAll($"user_id={User.Id}")).ToList();

            if (Operations.Count > 0)
            {
                var filter = "";
                foreach (var operation in Operations)
                {
                    filter += $"operation_id={operation.Id} AND ";
                }
                filter = filter.Substring(0, filter.Length - 4);

                var relations = new RelationMapper(Operations, Tags).ConvertToBusinessLogicEntitiesCollection(OperationTagRelationAdapter.GetAll(filter));
                var relTags = relations.Item1;
                var relOperations = relations.Item2;

                foreach (var operation in Operations)
                {
                    operation.SetDetailsList(new OperationDetailsMapper().ConvertToBusinessLogicEntitiesCollection(OperationDetailsAdapter.GetAll($"operaiton_id={operation.Id}")));
                    operation.SetTags(relTags.Where(r => r.OperationId == operation.Id));
                }

                foreach (var tag in Tags)
                {
                    tag.SetOperations(relOperations.Where(r => r.TagId == tag.Id));
                }
            }
        }

        public void Import()
        {
            var ignored = new List<string> {
                " przelew Smart Saver Płatność kartą [0-9]{2}.[0-9]{2}.[0-9]{4} Nr karty 4246xx4642 Kwota: [0-9]+,[0-9]{2} [A-Z]{3}"
            };

            var importFactory = ImportFactory.GetFactory("ING");
            IImporter importer = importFactory.CreateImporter();

            var transactions = importer.ReadTransactions();
            foreach (var ignoredItem in ignored)
            {
                transactions = transactions.Where(t => !Regex.Match(t.Title, ignoredItem).Success);
            }

            var operations = new TransactionToOperationMapper().ConvertToEntitiesCollection(transactions, User, TransactionTypes, TransferTypes).ToList();
            operations.ForEach(o=>o.IsDirty=true);

            operations[0].SetFrequence(Frequencies[4]);
            operations[0].SetImportance(Importances[2]);
            // operations[0].SetTags(new List<RelTag> { new RelTag(-1,Tags[4], operations[0].Id)});

            InsertOperation(operations[0]);
        }

        private void InsertOperation(Operation operation)
        {
            var id = OperationAdapter.Insert(new OperationMapper(User, Importances, Frequencies, TransactionTypes, TransferTypes).ConvertToDALEntity(operation));
            operation.IsDirty = false;
            operation.UpdateId(id);
            //OperationTagAdapter.Insert(new RelationMapper(Operations, Tags).ConvertToDALEntity(operation));
        }
    }
}
