using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;

namespace ClassGenerator
{
    /** \ingroup Bll
    <summary>
    This class represents generated FileManager class
        that manages IO operations on db via coding.
         Also part of \ref Builder
    </summary>
        */
    internal class SaveClass : CreatedClass
   {
       protected string generateDirectory;
       public SaveClass(string directory, string name, string generateDirectory) : base(directory, name)
       {
            this.generateDirectory = generateDirectory;
       }
       public override void BuildMethods()
       {
           base.BuildMethods();
           LoadMethod();
           SaveMethod();
           GetAttribute();
           CreateGenericType(new List<GenericClassParameters>() { new GenericClassParameters() { GenericType = "T", BaseType="IChangeVisitor", IsBaseTypeGeneric=true, Constraint=new List<string>() { "ITable", "new()"} } });
           
           
       }
       /// <summary>
       /// Generates constructor with format parameter
       /// and saves it to local format variable(type string)
       /// </summary>
       public override void BuildOtherConstructors()
       {
           base.BuildOtherConstructors();
           CodeConstructor codeConstructor = new CodeConstructor();
           codeConstructor.Parameters.Add(new CodeParameterDeclarationExpression("System.String", "format"));
           codeConstructor.Attributes = MemberAttributes.Public;
           CodeStatement FormatAssignmentStatement = GetAssignment("Format", new KeyValuePair<string, bool>("format", true));
           codeConstructor.Statements.Add(FormatAssignmentStatement);
           TheType.Members.Add(codeConstructor);
       }

       /* Creates GetAttribute method in a class.
         This method works with xml and get's value of attribute in specified XElement
       */
    private void GetAttribute()
        {
            CodeMemberMethod GetAttributeMethod = BuildMethodSignature("GetAttribute", MemberAttributes.Public, new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>>() { new System.Collections.Generic.KeyValuePair<string, string>("XElement", "xe"), new KeyValuePair<string, string>("System.String", "name") }, "Syste.String");
            CodeExpression AttributeMethodInvokationExpression = GetMethodInvokationExpression("xe", "Attribute", new List<KeyValuePair<string, bool>>() { new KeyValuePair<string, bool>("name", true) });

            CodeStatement AttributeAssignmentStatement = GetAssignment("XAttribute", "attr", AttributeMethodInvokationExpression);
            GetAttributeMethod.Statements.Add(AttributeAssignmentStatement);
            CodeStatement AttributeNotNullCheckStatement = GetUnoConditionIf("attr", CodeBinaryOperatorType.IdentityEquality, "null", new List<CodeStatement>() { new CodeMethodReturnStatement(new CodeVariableReferenceExpression("string.Empty")) });
      
            CodeExpression AttributNotNullAdmitStatement = GetConditionCompareToNull("attr");
            CodeStatement ExpressionStatement = new CodeExpressionStatement(AttributNotNullAdmitStatement);
            GetAttributeMethod.Statements.Add(AttributeNotNullCheckStatement);
            CodeExpression ValuePropertyExpression = new CodeVariableReferenceExpression("attr.Value");
            CodeStatement ValueAssignmentExpression = GetAssignment("System.String", "value", ValuePropertyExpression);
            GetAttributeMethod.Statements.Add(ValueAssignmentExpression);
            GetAttributeMethod.Statements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression("value")));
            TheType.Members.Add(GetAttributeMethod);
        }

        ///<summary>
        /// Generate method to Load data from database
        /// to memory(this method only calls to specific Load methods)
        /// </summary>
        protected void LoadMethod()
        {
            CodeMemberMethod cmm = BuildMethodSignature("LoadData", MemberAttributes.Public, new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>>() { new System.Collections.Generic.KeyValuePair<string, string>("TableList<T>", "table") });
            TheType.Members.Add(cmm);
            CodeStatement csLoadOtherTypesMethod = GetMethodInvokationStatement("this", "LoadOtherTypes", new List<string>() { "table" });
            CodeStatement csLoadJsonMethod = GetMethodInvokationStatement("this", "LoadJson", new List<string>() { "table" });
          
            CodeStatement csLoadXmlMethod = GetMethodInvokationStatement("this", "LoadXml", new List<string>() { "table" });
            CodeStatement csIfFormatXml = GetUnoConditionIf("this.Format", CodeBinaryOperatorType.ValueEquality, "XML", false, new List<CodeStatement>() {csLoadXmlMethod }, new List<CodeStatement>() { csLoadOtherTypesMethod});
            cmm.Statements.Add(csIfFormatXml);
            LoadXml();
        }

        /// <summary>
        ///  Generates methods for loading data from xml files and other types
       /// to memory
        /// </summary>
       
        public void LoadXml()
        {
            CodeMemberMethod LoadXmlMethod = BuildMethodSignature("LoadXml", MemberAttributes.Public, new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>>() { new System.Collections.Generic.KeyValuePair<string, string>("TableList<T>", "table") });
            TheType.Members.Add(LoadXmlMethod);
            CodeStatement TableNameAssignment = GetAssignment("System.String", "tableName", new CodeVariableReferenceExpression("table.Name"));
            LoadXmlMethod.Statements.Add(TableNameAssignment);
           // CodeStatement cas12 = GetMethodInvokationAssignment("string", "fileName", "string", "Format", new System.Collections.Generic.List<KeyValuePair<string, bool>>() { new KeyValuePair<string, bool>("{0}.{1}", false), new KeyValuePair<string, bool>("tableName", true), new KeyValuePair<string, bool>("table.Format", true) });
            CodeExpression FormatMethodInvokationStatement = GetMethodInvokationExpression("System.String", "Format", new System.Collections.Generic.List<KeyValuePair<string, bool>>() { new KeyValuePair<string, bool>("{0}\\\\Files\\\\{1}.{2}", false), new KeyValuePair<string, bool>(generateDirectory, false), new KeyValuePair<string, bool>("tableName", true), new KeyValuePair<string, bool>("Format", true) });
            CodeStatement FileNameAssignmentStatement = GetAssignment("System.String", "fileName", FormatMethodInvokationStatement);
            LoadXmlMethod.Statements.Add(FileNameAssignmentStatement);
            CodeExpression LoadMethodInvokationStatement = GetMethodInvokationExpression("XDocument", "Load", new System.Collections.Generic.List<KeyValuePair<string, bool>> { new KeyValuePair<string, bool>("fileName", true) });

            CodeStatement XmlAssignmentStatement = GetAssignment("XDocument", "xdTable", LoadMethodInvokationStatement);
            CodeStatement DataElementNameAssignmentStatement = GetAssignment("System.String", "xmlDataElement", new CodePrimitiveExpression("Data"));
            CodeExpression ElementMethodInvokationExpression = GetMethodInvokationExpression("xdTable.Root", "Element", new System.Collections.Generic.List<KeyValuePair<string, bool>>() { new KeyValuePair<string, bool>("xmlDataElement", true) });
            CodeStatement DataElementAssignmentStatement = GetAssignment("XElement", "xu", ElementMethodInvokationExpression);
            CodeStatement CheckThatDataExistsStatement = GetUnoConditionIf("xu", CodeBinaryOperatorType.ValueEquality, "null", new List<CodeStatement>() { new CodeMethodReturnStatement() });
            CodeExpression ElementsXmlSearchExpression = GetMethodInvokationExpression("xu", "Elements", new List<KeyValuePair<string, bool>>() { new KeyValuePair<string, bool>("Elements", false) });
            CodeStatement ElementsAssignmentStatement = GetAssignment("IEnumerable<XElement>", "elements", ElementsXmlSearchExpression);
            CodeExpression ToListMethodInvokationExpression = GetMethodInvokationExpression("elements", "ToList", new List<KeyValuePair<string, bool>>());
            CodeStatement ElementListAssignmentStatement = GetAssignment("List<XElement>", "lstElements", ToListMethodInvokationExpression);
            CodeStatement CollectionAddMethodnvokationStatement = GetMethodInvokationStatement("table", "Add", new List<string>() { "xu.Elements(\"Element\")[i]" });
           
            LoadXmlMethod.Statements.Add(DataElementNameAssignmentStatement);
            LoadXmlMethod.Statements.Add(XmlAssignmentStatement);
            LoadXmlMethod.Statements.Add(DataElementAssignmentStatement);
            LoadXmlMethod.Statements.Add(ElementMethodInvokationExpression);
            LoadXmlMethod.Statements.Add(CheckThatDataExistsStatement);
            LoadXmlMethod.Statements.Add(ElementsAssignmentStatement);
            LoadXmlMethod.Statements.Add(ElementListAssignmentStatement);
            //LoadXmlMethod.Statements.Add(CollectionAddMethodnvokationStatement);
            List<CodeStatement> lstLoadDbRecordsStatements = CreateLoadXmlStatements("lstElements", "i");
            CodeCompoundStatement LoadDbRecordCycleStatement = GetSimpleForInteger("i", new System.Collections.Generic.KeyValuePair<int, string>(0, "lstElements.Count"), 1, lstLoadDbRecordsStatements);
            LoadXmlMethod.Statements.AddRange(LoadDbRecordCycleStatement.Statements.ToArray());
            CodeMemberMethod LoadJsonMethod = BuildMethodSignature("LoadJson", MemberAttributes.Public, new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>>() { new System.Collections.Generic.KeyValuePair<string, string>("TableList<T>", "table") });
            TheType.Members.Add(LoadJsonMethod);
            CodeMemberMethod LoadOtherTypesMethod = BuildMethodSignature("LoadOtherTypes", MemberAttributes.Public, new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>>() { new System.Collections.Generic.KeyValuePair<string, string>("TableList<T>", "table") });
            TheType.Members.Add(LoadOtherTypesMethod);
        }

       /// <summary>
       /// Generates cycle statements of loading each db record to memory
       /// (Find in xml and convert it to appropriate object)
       /// </summary>
       /// <param name="elementsName">Name of the elements collection variable</param>
       /// <param name="indexName">Name of cycle index(commonly i)</param>
       /// <returns>List of statements for a cycle for</returns>
        private List<CodeStatement> CreateLoadXmlStatements(string elementsName, string indexName)
        {
            List<CodeStatement> lst = new List<CodeStatement>();
            // Declaration of record(One line in db table for example)
            CodeStatement RecordDeclarationStatement = GetDeclaration("record", "T");
            lst.Add(RecordDeclarationStatement);
            CodeStatement RecordObjectCreationStatement = GetObjectCreateStatement("T", "record", true);
            lst.Add(RecordObjectCreationStatement);
            CodeStatement CurrentElementAssignStatement = GetAssignment("XElement", "xeCurrent", new CodeVariableReferenceExpression(string.Format("{0}[{1}]", elementsName, indexName)));
            lst.Add(CurrentElementAssignStatement);
            CodeExpression SearchPropertyElementExpression = GetMethodInvokationExpression("xeCurrent", "Elements", new System.Collections.Generic.List<KeyValuePair<string, bool>>() { new KeyValuePair<string, bool>("Property", false) });
            CodeStatement PropertyXElementAssignStatement = GetAssignment("IEnumerable<XElement>", "xeProperties", SearchPropertyElementExpression);
            lst.Add(PropertyXElementAssignStatement);
            CodeStatement PropertiesListAssignmentStatement = GetAssignment("List<XElement>", "lstProperties", GetMethodInvokationExpression("xeProperties", "ToList", new List<KeyValuePair<string, bool>>()));
            lst.Add(PropertiesListAssignmentStatement);
            List<CodeStatement> lstPropertyStatements = CreateLoadPropertyStatements("lstProperties", "j");
            CodeCompoundStatement PropertyLoadCycleStatement = GetSimpleForInteger("j", new System.Collections.Generic.KeyValuePair<int, string>(0, "lstProperties.Count"), 1, lstPropertyStatements);
            lst.AddRange(PropertyLoadCycleStatement.Statements.ToArray());
            CodeStatement CollectionAddInvokationStatement = GetMethodInvokationStatement("table", "Add", new List<string>() { "record" });
            lst.Add(CollectionAddInvokationStatement);
            return lst;
        }


        /// <summary>
        /// Generating the code to load each property from xml to current record in memory(element in collection)
        /// </summary>
        /// <param name="propertyElementName">Name of variable that is collection of properties</param>
        /// <param name="indexName">Name of index for cycle(index)</param>
        /// <returns>List of load property statements(for inner for cycle)</returns>
        private List<CodeStatement> CreateLoadPropertyStatements(string propertyElementName, string indexName)
        {
            List<CodeStatement> lst = new List<CodeStatement>();
            CodeStatement CurrentPropertyXElementAssignStatement = GetAssignment("XElement", "xeCurrentProperty", new CodeVariableReferenceExpression(string.Format("{0}[{1}]", propertyElementName, indexName)));
            lst.Add(CurrentPropertyXElementAssignStatement);

            CodeExpression GetAttributeValueInvokationExpression = GetMethodInvokationExpression("this", "GetAttribute", new System.Collections.Generic.List<KeyValuePair<string, bool>>() { new KeyValuePair<string, bool>("xeCurrentProperty", true), new KeyValuePair<string, bool>("PropertyValue", false) });
            CodeExpression GetAttributeNameInvokeExpression = GetMethodInvokationExpression("this", "GetAttribute", new System.Collections.Generic.List<KeyValuePair<string, bool>>() { new KeyValuePair<string, bool>("xeCurrentProperty", true), new KeyValuePair<string, bool>("PropertyName", false) });
            CodeStatement PropertyValueAssignStatement = GetAssignment("System.String", "propertyValue", GetAttributeValueInvokationExpression);
            CodeStatement PropertyNameAssignStatement = GetAssignment("System.String", "propertyName", GetAttributeNameInvokeExpression);
            lst.Add(PropertyNameAssignStatement);
            lst.Add(PropertyValueAssignStatement);
            
            CodeExpression GetTypeInvokeExpression = GetMethodInvokationExpression("record", "GetType", new List<KeyValuePair<string, bool>>());
            CodeStatement RecordTypeAssignStatement = GetAssignment("Type", "recordType", GetTypeInvokeExpression);
            lst.Add(RecordTypeAssignStatement);
            CodeExpression GetPropertyInvokationExpression = GetMethodInvokationExpression("recordType", "GetProperty", new List<KeyValuePair<string, bool>>() { new KeyValuePair<string, bool>("propertyName",true)});
            CodeStatement CurrentPropertyAssignStatement = GetAssignment("PropertyInfo", "currentProperty", GetPropertyInvokationExpression);
            lst.Add(CurrentPropertyAssignStatement);
           
            CodeStatement CurrentPropertyTypeAssignStatement = GetAssignment("Type", "currentPropertyType", new CodeVariableReferenceExpression("currentProperty.PropertyType"));
            lst.Add(CurrentPropertyTypeAssignStatement);
            CodeExpression ToStringMethodInvokeExpression = GetMethodInvokationExpression("currentPropertyType", "ToString", new List<KeyValuePair<string,bool>>() );
            CodeStatement PropertyTypeStringAssignStatement = GetAssignment("System.String", "propTypeStr", ToStringMethodInvokeExpression);
            //lst.Add(PropertyTypeStringAssignStatement);
            CodeExpression CreateInstanceInvokationExpression = GetMethodInvokationExpression("Activator", "CreateInstance", new List<KeyValuePair<string, bool>>() { new KeyValuePair<string, bool>("currentPropertyType", true), new KeyValuePair<string, bool>("propertyValue", true) });
            CodeStatement PropertyObjectAssignStatement = GetAssignmentWithExpression("objProperty", CreateInstanceInvokationExpression);
          
            CodeStatement SetValueInvokationStatement = GetMethodInvokationStatement("currentProperty", "SetValue", new List<string>() { "record", "objProperty" });
            CodeStatement SetValueInvokationStatement0 = GetMethodInvokationStatement("pp", "SetValue", new List<string>() { "PropertyObject", "objProperty" });
            //lst.Add(PropertyObjectAssignStatement);
            CodeExpression ce = GetMethodInvokationExpression("TypeDescriptor", "GetConverter", new List<CodeExpression>() { new CodeVariableReferenceExpression("currentPropertyType") });
            CodeStatement cs = GetAssignment("TypeConverter", "tcConvert", ce);
            CodeExpression ce1 = GetMethodInvokationExpression("tcConvert", "ConvertFromString", new List<CodeExpression>() { new CodeVariableReferenceExpression("propertyValue") });
            CodeStatement cs1 = GetAssignmentWithExpression("objProperty", ce1);
            CodeStatement cs0 = GetDeclaration("objProperty", "System.Object");
            lst.Add(cs0);
            CodeExpression cee0 = GetMethodInvokationExpression("currentProperty", "GetValue", new List<CodeExpression>() { new CodeVariableReferenceExpression("record") });
            CodeStatement cse0 = GetAssignment("System.Object", "PropertyObject", cee0);
            CodeExpression cee = GetMethodInvokationExpression("currentPropertyType", "GetProperty", new List<CodeExpression>() { new CodePrimitiveExpression("Value") });
            CodeStatement cse = GetAssignment("PropertyInfo", "pp", cee);
            CodeExpression ce2 = GetMethodInvokationExpression("TypeDescriptor", "GetConverter", new List<CodeExpression>() { new CodeVariableReferenceExpression("pp.PropertyType") });
            CodeStatement cs2 = GetAssignment("TypeConverter", "tcConvert", ce2);
            CodeStatement cs3 = GetAssignmentWithExpression("objProperty", ce1);
            CodeStatement csIfValueType = GetUnoConditionIf("currentPropertyType.BaseType.Name", CodeBinaryOperatorType.IdentityInequality, "ValueType", false, new List<CodeStatement>() { cse0, cse, cs2, cs3, SetValueInvokationStatement0 }, new List<CodeStatement>() { cs, cs1, SetValueInvokationStatement });
            lst.Add(csIfValueType);
           // lst.Add(SetValueInvokationStatement);
           
            return lst;
        }

        /// <summary>
        /// Generate a save method which checks format and
        /// calls appropriate save function according to format
        /// </summary>
        private void SaveMethod()
        {
            CodeMemberMethod SaveDataMethod = BuildMethodSignature("SaveData", MemberAttributes.Public, new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>>() { new System.Collections.Generic.KeyValuePair<string, string>("TableList<T>", "table") });
            TheType.Members.Add(SaveDataMethod);
            CodeStatement csSaveOtherTypesMethod = GetMethodInvokationStatement("this", "SaveOtherTypes", new List<string>() { "table" });
           
           
            CodeStatement csSaveXmlMethod = GetMethodInvokationStatement("this", "SaveXml", new List<string>() { "table" });
            CodeStatement csIfFormatXml = GetUnoConditionIf("this.Format", CodeBinaryOperatorType.ValueEquality, "XML", false, new List<CodeStatement>() { csSaveXmlMethod }, new List<CodeStatement>() { csSaveOtherTypesMethod });
            SaveDataMethod.Statements.Add(csIfFormatXml);
            SaveXml();
        }

        /// <summary>
        /// Generates method that saves records of table to xml file
        /// </summary>
        private void SaveXml()
        {
            CodeMemberMethod SaveXmlmethod = BuildMethodSignature("SaveXml", MemberAttributes.Public, new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>>() { new System.Collections.Generic.KeyValuePair<string, string>("TableList<T>", "table") });
            TheType.Members.Add(SaveXmlmethod);
            CodeStatement TableNameAssignStatement = GetAssignment("System.String", "tableName", new CodeVariableReferenceExpression("table.Name"));
            SaveXmlmethod.Statements.Add(TableNameAssignStatement);
            // CodeStatement cas12 = GetMethodInvokationAssignment("string", "fileName", "string", "Format", new System.Collections.Generic.List<KeyValuePair<string, bool>>() { new KeyValuePair<string, bool>("{0}.{1}", false), new KeyValuePair<string, bool>("tableName", true), new KeyValuePair<string, bool>("table.Format", true) });
            CodeExpression FormatMethodInvokeStatement = GetMethodInvokationExpression("System.String", "Format", new System.Collections.Generic.List<KeyValuePair<string, bool>>() { new KeyValuePair<string, bool>("{0}\\\\Files\\\\{1}.{2}", false), new KeyValuePair<string, bool>(generateDirectory, false), new KeyValuePair<string, bool>("tableName", true), new KeyValuePair<string, bool>("Format", true) });
            CodeStatement FileNameAssignStatement = GetAssignment("System.String", "fileName", FormatMethodInvokeStatement);
            SaveXmlmethod.Statements.Add(FileNameAssignStatement);
            CodeExpression LoadMethodInvokeExpression = GetMethodInvokationExpression("XDocument", "Load", new System.Collections.Generic.List<KeyValuePair<string, bool>> { new KeyValuePair<string, bool>("fileName", true) });

            CodeStatement TableXElementAssignmentStatement = GetAssignment("XDocument", "xdTable", LoadMethodInvokeExpression);
            CodeStatement DataElementXElementAssignStatement = GetAssignment("System.String", "xmlDataElement", new CodePrimitiveExpression("Data"));
            CodeExpression SearchElementInXmlStatement = GetMethodInvokationExpression("xdTable.Root", "Element", new System.Collections.Generic.List<KeyValuePair<string, bool>>() { new KeyValuePair<string, bool>("xmlDataElement", true) });
            CodeStatement XElementAssignStatement = GetAssignment("XElement", "xu", SearchElementInXmlStatement);
            CodeStatement CreateDataElementXmlObjStatement = GetObjectCreateStatement("XElement", "xu", true, new List<string>() { "Data" });
            CodeStatement AddXElementToXmlStatement = GetMethodInvokationStatement("xdTable.Root", "Add", new List<string>() {"xu" });
            CodeStatement RemoveAllInvokationStatement = GetMethodInvokationStatement("xu", "RemoveAll", new List<string>());
            CodeStatement CheckRecordExistanceStatement = GetUnoConditionIf("xu", CodeBinaryOperatorType.ValueEquality, "null", new List<CodeStatement>() { CreateDataElementXmlObjStatement, AddXElementToXmlStatement }, new List<CodeStatement>() { RemoveAllInvokationStatement });
            CodeExpression RemoveMethodInvokationExpression = GetMethodInvokationExpression("xu.Elements()", "Remove", new List<KeyValuePair<string, bool>>());
            CodeStatement RemoveMethodInvokationStatement = new CodeExpressionStatement(RemoveMethodInvokationExpression);
            CodeStatement CollectionAddInvokationStatement = GetMethodInvokationStatement("table", "Add", new List<string>() { "xu.Elements(\"Element\")[i]" });
            CodeExpression SearchElementsXElementExpression = GetMethodInvokationExpression("xu", "Elements", new List<KeyValuePair<string, bool>>() { new KeyValuePair<string, bool>("Elements", false) });
            CodeStatement ElementAssignStatement = GetAssignment("IEnumerable<XElement>", "elements", SearchElementsXElementExpression);
            SaveXmlmethod.Statements.Add(TableXElementAssignmentStatement);
            SaveXmlmethod.Statements.Add(DataElementXElementAssignStatement);
            SaveXmlmethod.Statements.Add(XElementAssignStatement);
            SaveXmlmethod.Statements.Add(CheckRecordExistanceStatement);
            SaveXmlmethod.Statements.Add(ElementAssignStatement);
            SaveXmlmethod.Statements.Add(RemoveMethodInvokationStatement);
            List<CodeStatement> SaveRecordStatements = CreateSaveXmlStatements("table", "i");
            CodeCompoundStatement SaveRecordCycleStatement = GetSimpleForInteger("i", new System.Collections.Generic.KeyValuePair<int, string>(0, "table.Count"), 1, SaveRecordStatements);
            SaveXmlmethod.Statements.AddRange(SaveRecordCycleStatement.Statements.ToArray());
            CodeStatement XDocumentSaveInvokeStatement = GetMethodInvokationStatement("xdTable", "Save", new List<string>() { "fileName" });
            SaveXmlmethod.Statements.Add(XDocumentSaveInvokeStatement);
           
            CodeMemberMethod SaveOtherTypesMethod = BuildMethodSignature("SaveOtherTypes", MemberAttributes.Public, new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>>() { new System.Collections.Generic.KeyValuePair<string, string>("TableList<T>", "table") });
            TheType.Members.Add(SaveOtherTypesMethod);
        }

        /// <summary>
        /// Generate saving record statements for xml
        /// </summary>
        /// <param name="elementsName">Name of collection of records variable</param>
        /// <param name="indexName">Name of cycle index</param>
        /// <returns>list of cycle statements</returns>
        private List<CodeStatement> CreateSaveXmlStatements(string elementsName, string indexName)
        {
            List<CodeStatement> lst = new List<CodeStatement>();

            CodeStatement RecordDeclarationStatement = GetDeclaration("record", "T");
            lst.Add(RecordDeclarationStatement);
            CodeStatement RecordAssignStatement = GetAssignment("record", new KeyValuePair<string,bool>("table[i]",true));
            lst.Add(RecordAssignStatement);
            CodeExpression GetTypeInvokeStatement = GetMethodInvokationExpression("record", "GetType", new System.Collections.Generic.List<KeyValuePair<string, bool>>());
            CodeStatement TypeAssignStatement = GetAssignment("Type", "type", GetTypeInvokeStatement);
            lst.Add(TypeAssignStatement);
            CodeExpression GetPropertiesInvokeExpression = GetMethodInvokationExpression("type", "GetProperties", new System.Collections.Generic.List<KeyValuePair<string, bool>>());
            CodeStatement PropertiesAssignStatement = GetAssignment("PropertyInfo []", "pi", GetPropertiesInvokeExpression);
            lst.Add(PropertiesAssignStatement);
            CodeStatement CurrentDeclarationStatement = GetDeclaration("xeCurrent", "XElement");
            lst.Add(CurrentDeclarationStatement);
            CodeStatement CunbjcCreateStatement = GetObjectCreateStatement("XElement", "xeCurrent", true, new List<string>() { "Elements" });
            lst.Add(CunbjcCreateStatement);
            List<CodeStatement> lstSavePropertyStatements = CreateSavePropertyStatements("pi", "j");
            CodeCompoundStatement SavePropertyCycleStatement = GetSimpleForInteger("j", new System.Collections.Generic.KeyValuePair<int, string>(0, "pi.Length"), 1, lstSavePropertyStatements);
            lst.AddRange(SavePropertyCycleStatement.Statements.ToArray());
            CodeStatement XElementAddInvokationStatement = GetMethodInvokationStatement("xu", "Add", new List<string>() { "xeCurrent" });
            lst.Add(XElementAddInvokationStatement);
            return lst;
        }

        /// <summary>
        /// Generate inner cycle statements for saving table field value to xml
        /// </summary>
        /// <param name="propertyElementName">Name of proerty</param>
        /// <param name="indexName">Cycle index(commonly i,j, k)</param>
        /// <returns>List of cycle statements</returns>
        private List<CodeStatement> CreateSavePropertyStatements(string propertyElementName, string indexName)
        {
            List<CodeStatement> lst = new List<CodeStatement>();
            CodeStatement CurrentPropertyAssignStatement = GetAssignment("PropertyInfo", "pj", new CodeVariableReferenceExpression(string.Format("{0}[{1}]", propertyElementName, indexName)));
            lst.Add(CurrentPropertyAssignStatement);
            CodeStatement PropertyXElementDeclarationStatement = GetDeclaration("xeProperty", "XElement");
            lst.Add(PropertyXElementDeclarationStatement);
            CodeStatement PropertyXElementObjectCreateStatement = GetObjectCreateStatement("XElement", "xeProperty", true, new List<string>() { "Property" });
            lst.Add(PropertyXElementObjectCreateStatement);
            CodeExpression PropertyValueGetAttributeInvokeStatement = GetMethodInvokationExpression("this", "GetAttribute", new System.Collections.Generic.List<KeyValuePair<string, bool>>() { new KeyValuePair<string, bool>("xeProperty", true), new KeyValuePair<string, bool>("PropertyValue", false) });
            CodeExpression PropertyNameGetAttributeInvokeStatement = GetMethodInvokationExpression("this", "GetAttribute", new System.Collections.Generic.List<KeyValuePair<string, bool>>() { new KeyValuePair<string, bool>("xeProperty", true), new KeyValuePair<string, bool>("PropertyName", false) });
            CodeStatement NameDeclarationStatement = GetDeclaration("name", "System.String");
            lst.Add(NameDeclarationStatement);
            CodeStatement NameAssignStatement = GetAssignment("name", new KeyValuePair<string,bool>("pj.Name", true));
            lst.Add(NameAssignStatement);
            CodeStatement PropertyNameAssignmentStatement = GetAssignment("System.String", "propertyName", PropertyNameGetAttributeInvokeStatement);
            lst.Add(PropertyNameAssignmentStatement);
            CodeExpression PropertyGetValueInvokeStatement = GetMethodInvokationExpression("pj", "GetValue", new System.Collections.Generic.List<KeyValuePair<string, bool>>() { new KeyValuePair<string, bool>("record", true) });
            CodeStatement PropertyValueAssignmentStatement = GetAssignment("System.object", "pvalue", PropertyGetValueInvokeStatement);
            lst.Add(PropertyValueAssignmentStatement);
           
            CodeExpression PropertyTypeInvokeExpression = GetMethodInvokationExpression("pvalue", "GetType", new List<KeyValuePair<string,bool>>() );
            CodeStatement PropertyTypeAssignmentStatement = GetAssignment("Type", "tt", PropertyTypeInvokeExpression);
            lst.Add(PropertyTypeAssignmentStatement);
          
            CodeExpression ValueGetPropertyInvokationExpression = GetMethodInvokationExpression("tt", "GetProperty", new List<KeyValuePair<string, bool>>() { new KeyValuePair<string, bool>("Value", false)});
            CodeStatement ValuePropertyInfoAssignStatement = GetAssignment("PropertyInfo", "pw", ValueGetPropertyInvokationExpression);
            lst.Add(ValuePropertyInfoAssignStatement);
            CodeStatement csDeclare = GetDeclaration("value", "System.Object");
            lst.Add(csDeclare);
            CodeExpression ValueGetValueInvokeStatement = GetMethodInvokationExpression("pw", "GetValue", new List<KeyValuePair<string, bool>>() { new KeyValuePair<string, bool>("pvalue", true) });
            CodeStatement ValueAssignStatement = GetAssignmentWithExpression( "value", ValueGetValueInvokeStatement);
            //lst.Add(ValueAssignStatement);
            CodeStatement css= GetAssignment( "value", new KeyValuePair<string,bool> ("pvalue", true));
            CodeStatement csIf = GetUnoConditionIf("pw", CodeBinaryOperatorType.IdentityInequality, "null", new List<CodeStatement>() { ValueAssignStatement }, new List<CodeStatement>() { css });
            lst.Add(csIf);
            CodeExpression SetAttributeValueOfNameInvokationExpression = GetMethodInvokationExpression("xeProperty", "SetAttributeValue", new List<KeyValuePair<string, bool>>() { new KeyValuePair<string, bool>("PropertyName", false), new KeyValuePair<string, bool>("name", true) });
            CodeStatement SetAttributeValueNameInvokeStatement = new CodeExpressionStatement(SetAttributeValueOfNameInvokationExpression);
            lst.Add(SetAttributeValueNameInvokeStatement);
            CodeExpression SetAttributeValueValueInvokationExpression = GetMethodInvokationExpression("xeProperty", "SetAttributeValue", new List<KeyValuePair<string, bool>>() { new KeyValuePair<string, bool>("PropertyValue", false), new KeyValuePair<string, bool>("value", true) });
            CodeStatement SetAttributeValueValueInvokationStatement = new CodeExpressionStatement(SetAttributeValueValueInvokationExpression);
            lst.Add(SetAttributeValueValueInvokationStatement);
            CodeStatement XElementAddPropertyInvokeStatement = GetMethodInvokationStatement("xeCurrent", "Add", new List<string>() { "xeProperty"});
       
            lst.Add(XElementAddPropertyInvokeStatement);
          
            return lst;
        }
    }
}