<?xml version="1.0" encoding="utf-8"?>
<Dsl xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="2bad020f-9a7d-4ffa-8cc1-6050f57b0191" Description="Domain Specific Language for creating CQRS/ES domains" Name="CQRSdsl" DisplayName="CQRSdsl" Namespace="CQRSAzure.CQRSdsl.Dsl" MinorVersion="1" ProductName="CQRSdsl" CompanyName="CQRSAzure" PackageGuid="5f54fc45-4897-47e5-b71f-8ff66160144e" PackageNamespace="CQRSAzure.CQRSdsl.Dsl" xmlns="http://schemas.microsoft.com/VisualStudio/2005/DslTools/DslDefinitionModel">
  <Classes>
    <DomainClass Id="b927fa6e-0562-44cc-9be0-71281aa07d25" Description="The root in which all other elements are embedded. Appears as a diagram." Name="CQRSModel" DisplayName="CQRSModel" Namespace="CQRSAzure.CQRSdsl.Dsl" HasCustomConstructor="true">
      <Notes>The CQRS model describes a domain expressed using the Command Query responsability separation pattern</Notes>
      <Properties>
        <DomainProperty Id="693bbdfa-8599-4433-b4a2-6afc5aaae04e" Description="Name of the overall CQRS model" Name="Name" DisplayName="Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="1e6c5edc-40b6-41b4-b57f-63bb30dc9219" Description="Informal notes for documenting the entire CQRS domain" Name="Notes" DisplayName="Notes" Category="Documentation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="4499457a-4d6e-4e98-88a7-115b49bc09d7" Description="Description for this model" Name="Description" DisplayName="Description" Category="Documentation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="AggregateIdentifier" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>CQRSModelHasAggregateIdentifiers.AggregateIdentifiers</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="ModelSetting" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>CQRSModelHasModelSet.ModelSet</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="3d71fb67-1375-4f99-8206-12a78e514a4a" Description="Class to uniquely identiy a particular aggregate type" Name="AggregateIdentifier" DisplayName="Aggregate Identifier" Namespace="CQRSAzure.CQRSdsl.Dsl">
      <Properties>
        <DomainProperty Id="51edf60d-da76-4479-8550-6d5ed806ccc0" Description="Unique name of the aggregate entity" Name="Name" DisplayName="Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="118003e6-14d5-441b-b65b-dbde1c84100d" Description="Description of this aggregate" Name="Description" DisplayName="Description">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="aff8fbc1-7274-4c00-8986-71a53279c166" Description="The name of the property that uniquely identifies this aggregate" Name="KeyName" DisplayName="Key Name" DefaultValue="Key" Category="Key">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="012239b1-8d7c-4678-92aa-b595efdee9fc" Description="The data type used for the field that uniquely " Name="KeyDataType" DisplayName="Key Data Type" DefaultValue="SystemGUID" Category="Key">
          <Type>
            <DomainEnumerationMoniker Name="KeyDataType" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="5006e794-744e-47eb-9329-bbc53656c368" Description="Additional notes for this aggregate identifier type" Name="Notes" DisplayName="Notes" Category="Documentation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="9e8c8044-3c57-4d6f-94c0-60d7242e52d6" Description="A tag /category that can be applied to an entity" Name="Category" DisplayName="Category">
          <Notes>This allows showing/hiding things by category (also known as onion-skinning)</Notes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="EventDefinition" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>AggregateIdentifierHasEventDefinitions.EventDefinitions</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="ProjectionDefinition" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>AggregateIdentifierHasProjectionDefinitions.ProjectionDefinitions</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="CommandDefinition" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>AggregateIdentifierHasCommandDefinitions.CommandDefinitions</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="QueryDefinition" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>AggregateIdentifierHasQueryDefinitions.QueryDefinitions</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="IdentityGroup" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>AggregateIdentifierHasIdentityGrouped.IdentityGrouped</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Classifier" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>AggregateIdentifierHasClassifiers.Classifiers</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="a88729a0-1121-458a-b5c7-32ef0714b62a" Description="Definition of an event that can occur to the aggregate" Name="EventDefinition" DisplayName="Event Definition" HelpKeyword="Event" Namespace="CQRSAzure.CQRSdsl.Dsl">
      <Properties>
        <DomainProperty Id="4c8f5eab-0160-4c63-848c-43251f566ae1" Description="Unique name of the event" Name="Name" DisplayName="Name" IsElementName="true">
          <Notes>This should ideally be a past tense noun or noun phrase.</Notes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="d4bde3de-baff-4fec-b6f5-eeee0b0904e4" Description="Business definition of what this event is that has occured for the aggregate" Name="Description" DisplayName="Description">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="c7083ddd-63ae-4ddf-a474-a7b7cf436730" Description="Additional notes for this event type" Name="Notes" DisplayName="Notes">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="3d92cab4-b21f-467f-abbc-3508a8e54786" Description="The current version of this event definition" Name="Version" DisplayName="Version" SetterAccessModifier="FamilyOrAssembly" IsUIReadOnly="true">
          <Notes>This is the version number of the event definition.  This allows teh definition of an event to be modified over time and for the handlers of the event to mofiy their behaviour accordingly.</Notes>
          <Type>
            <ExternalTypeMoniker Name="/System/Int32" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="089efc4b-197a-4e64-88be-e5fb500d3d7a" Description="A tag /category that can be applied to an entity" Name="Category" DisplayName="Category">
          <Notes>This allows showing/hiding things by category (also known as onion-skinning)</Notes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="EventProperty" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>EventDefinitionHasEventProperties.EventProperties</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="a506bd6f-43f3-44ea-b88e-66d3aaa85077" Description="Definition of a projection that can be run for this aggregate" Name="ProjectionDefinition" DisplayName="Projection Definition" Namespace="CQRSAzure.CQRSdsl.Dsl">
      <Notes>A projection is akin to a view of the state of the aggregate as derived from handling one or more events in the aggregate's event stream</Notes>
      <Properties>
        <DomainProperty Id="77d1c2a6-1c0b-4392-b592-7c0ed3d2730a" Description="Description for CQRSAzure.CQRSdsl.Dsl.ProjectionDefinition.Name" Name="Name" DisplayName="Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="0c867d88-f1b3-4de0-a4d0-a3fb58588bfc" Description="Business definition of what this projection shows about the aggregate" Name="Description" DisplayName="Description">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="22b4d69e-7ae9-4505-8f38-628fb1015970" Description="Additional notes for this projection view" Name="Notes" DisplayName="Notes">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="674acc98-a46d-478f-ad7f-f19ffc887c1e" Description="A tag /category that can be applied to an entity" Name="Category" DisplayName="Category">
          <Notes>This allows showing/hiding things by category (also known as onion-skinning)</Notes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="ProjectionProperty" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>ProjectionDefinitionHasProjectionProperties.ProjectionProperties</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="ProjectionEventPropertyOperation" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>ProjectionDefinitionHasEventPropertyOperations.ProjectionEventPropertyOperations</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="feed9883-9553-42cc-8b70-12dcd5032804" Description="Definition of a command that can raise one or more events for an aggregate root" Name="CommandDefinition" DisplayName="Command Definition" HelpKeyword="Command" Namespace="CQRSAzure.CQRSdsl.Dsl">
      <Properties>
        <DomainProperty Id="1f140a48-d40e-4801-b678-e4c507d9cb97" Description="Definition of a command that can be issued to the system" Name="Name" DisplayName="Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="438132b6-f7df-4be8-9906-fb4138543c05" Description="Business description of the command" Name="Description" DisplayName="Description" Category="Documentation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="4987db28-98b1-4522-8239-4e60e778ad30" Description="Additional notes for the command" Name="Notes" DisplayName="Notes" Category="Documentation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="70b7f601-70ac-4415-8c6b-ae0cab68f9ad" Description="A tag /category that can be applied to an entity" Name="Category" DisplayName="Category">
          <Notes>This allows showing/hiding things by category (also known as onion-skinning)</Notes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="CommandParameter" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>CommandDefinitionHasParameters.CommandParameters</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="1366ba0d-80c1-4315-a210-e8c478c524ff" Description="A property providing extra information about an event that has occured" Name="EventProperty" DisplayName="Event Property" Namespace="CQRSAzure.CQRSdsl.Dsl">
      <Properties>
        <DomainProperty Id="9dfb3392-ba6e-4327-be83-773a5c95b074" Description="The unique name of the property" Name="Name" DisplayName="Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="6816ac97-5bb9-4170-9441-3df89984303e" Description="The business description of the event property" Name="Description" DisplayName="Description">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="fc33344d-29a5-4b7f-b442-6ab06667abc7" Description="The backing data type of this event property" Name="DataType" DisplayName="Data Type" DefaultValue="Integer">
          <Type>
            <DomainEnumerationMoniker Name="PropertyDataType" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="c95623ab-e3e6-42ce-9e83-e34f0e17e512" Description="Additional notes for this event property" Name="Notes" DisplayName="Notes" Category="Documentation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="1b5f2aa3-d343-46a4-8dc0-7af37530b0db" Description="A property providing the data component of a projection" Name="ProjectionProperty" DisplayName="Projection Property" Namespace="CQRSAzure.CQRSdsl.Dsl">
      <Properties>
        <DomainProperty Id="2fed8a86-a04c-4809-8ee6-a9b7d265d51a" Description="Description for CQRSAzure.CQRSdsl.Dsl.ProjectionProperty.Name" Name="Name" DisplayName="Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="809018a6-3eee-4c10-8ac2-9540bd062488" Description="The business description of this property of the projection" Name="Description" DisplayName="Description" Category="Documentation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e98d8c8d-8e2c-4173-80e9-fb3e9498ca6d" Description="The data type backing this projection property definition" Name="DataType" DisplayName="Data Type" DefaultValue="Integer">
          <Type>
            <DomainEnumerationMoniker Name="PropertyDataType" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="24380e87-145d-4d69-a50e-d10f095cd6b1" Description="Additional notes for this projection property" Name="Notes" DisplayName="Notes" Category="Documentation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="520af4e5-6118-4933-bd60-8c03c96d4f42" Description="A parameter passed along with a command definition" Name="CommandParameter" DisplayName="Command Parameter" Namespace="CQRSAzure.CQRSdsl.Dsl">
      <Notes>This defines the data passed into the command handler</Notes>
      <Properties>
        <DomainProperty Id="99b9c142-89e5-4243-b7ec-5ff1bf1dd119" Description="Description for CQRSAzure.CQRSdsl.Dsl.CommandParameter.Name" Name="Name" DisplayName="Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="398cfa6b-91a3-4f1a-9e76-259cf4d497ac" Description="The business description of the command parameter" Name="Description" DisplayName="Description" Category="Documentation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="077c70e5-e23b-4df2-b918-47f8728b9a6c" Description="The backing data type for this parameter" Name="ParameterType" DisplayName="Parameter Type">
          <Type>
            <DomainEnumerationMoniker Name="PropertyDataType" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="d32d04c8-8bd8-4a67-84df-3cbea1952d89" Description="Additional notes for the command input parameter" Name="Notes" DisplayName="Notes" Category="Documentation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="d1b40286-c05e-47e5-ba4b-067c3e9bcd47" Description="Does this command parameter identify the aggregate that the command should apply to?" Name="IsAggregateKey" DisplayName="Is Aggregate Key" DefaultValue="False">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="4b891bb8-8eb6-41d0-8d6a-79125b91b74b" Description="Definition of a query that can be executed against an aggregate identifier in this domain" Name="QueryDefinition" DisplayName="Query Definition" Namespace="CQRSAzure.CQRSdsl.Dsl">
      <Properties>
        <DomainProperty Id="54e04f71-4233-431d-98c9-64c32a62e5a7" Description="Description for CQRSAzure.CQRSdsl.Dsl.QueryDefinition.Name" Name="Name" DisplayName="Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="a7cfafbf-27b3-4aee-9b48-258a375f9601" Description="Business description of this query definition" Name="Description" DisplayName="Description" Category="Documentation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="97eab1fa-0216-473e-a001-78ca407017df" Description="Does the query return an array of results" Name="MultiRowResults" DisplayName="Multi Row Results" DefaultValue="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="c5a44773-5f9c-4992-a4bc-1b970ee0c068" Description="Additional documentation for this query definition" Name="Notes" DisplayName="Notes" Category="Documentation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="112231f5-0c28-4341-b88f-9f137bf29aa3" Description="A tag /category that can be applied to an entity" Name="Category" DisplayName="Category">
          <Notes>This allows showing/hiding things by category (also known as onion-skinning)</Notes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="QueryInputParameter" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>QueryDefinitionHasQueryInputParameters.QueryInputParameters</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="QueryReturnParameter" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>QueryDefinitionHasQueryReturnParameters.QueryReturnParameters</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="9297dbdd-de1d-4bc3-ab44-53ec3003cafc" Description="A parameter to pass to a query" Name="QueryInputParameter" DisplayName="Query Input Parameter" Namespace="CQRSAzure.CQRSdsl.Dsl">
      <Notes>This is modelled separately to the query return properties</Notes>
      <Properties>
        <DomainProperty Id="8cd6127e-f8d9-4a21-be03-9f0163287964" Description="Description for CQRSAzure.CQRSdsl.Dsl.QueryInputParameter.Name" Name="Name" DisplayName="Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="d340e32c-fa4b-446a-9bce-f68890cb1016" Description="Business description of this query input parameter" Name="Description" DisplayName="Description" Category="Documentation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="058b63a8-2f79-45c2-b410-0b50b2db405c" Description="Backing data type of the input parameter" Name="DataType" DisplayName="Data Type">
          <Type>
            <DomainEnumerationMoniker Name="PropertyDataType" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="371838f0-7d84-44f0-a1b3-cb8f2d2a497a" Description="Is this query parameter the unique key of the aggregate the query is to run against?" Name="IsAggregateKey" DisplayName="Is Aggregate Key" DefaultValue="False">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="9356931b-df73-42c5-a719-86c4e1de2f72" Description="Additional notes for the query input parameter" Name="Notes" DisplayName="Notes" Category="Documentation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="fc68874a-f57b-40b7-92f7-3259fa342797" Description="Is this parameter an &quot;as-of&quot; indicator to tell the query when to stop reading the underlying event stream?" Name="IsEffectiveDate" DisplayName="Is Effective Date" DefaultValue="False">
          <Notes>If no "as-of" parameter is specified then up to the current end of stream is assumed</Notes>
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="5a0a5db7-8b56-49a4-afc4-51b42ad2e058" Description="An operation to perform on a projection property when an event is handled by the projection" Name="ProjectionEventPropertyOperation" DisplayName="Projection Event Property Operation" Namespace="CQRSAzure.CQRSdsl.Dsl">
      <Notes>This handles the basic operations for more complex scenarios custom code will need to be written</Notes>
      <Properties>
        <DomainProperty Id="ab9f4adb-48b1-4b79-84e7-907c01d86664" Description="The name of the event that this property operation occurs in handling" Name="EventName" DisplayName="Event Name">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(CQRSAzure.CQRSdsl.Dsl.CustomCode.UI.ProjectionPropertyOperationEventNameUITypeEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b5e68a29-6f6f-41e3-b234-1ebdda873de5" Description="The name of the property of the event (if any) used in the operation" Name="SourceEventPropertyName" DisplayName="Source Event Property Name">
          <Notes>This can be blank if the property operation does not require a source property</Notes>
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(CQRSAzure.CQRSdsl.Dsl.CustomCode.UI.ProjectionPropertyOperationSourceFieldUITypeEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="a0e40c01-4bce-41c5-a0c3-5a31c207ee12" Description="The name of the projection property " Name="TargetPropertyName" DisplayName="Target Property Name">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(CQRSAzure.CQRSdsl.Dsl.CustomCode.UI.ProjectionPropertyOperationTargetFieldUITypeEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="a0993e7b-4999-4640-8811-4bfcb50f2d7a" Description="What operation to perform to the target property of the projection" Name="PropertyOperationToPerform" DisplayName="Property Operation To Perform" DefaultValue="SetToValue">
          <Type>
            <DomainEnumerationMoniker Name="PropertyOperation" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="f6d24ecc-7603-4b93-9b1f-c16399354f1e" Description="Optional comment to document the property operation" Name="Description" DisplayName="Description" Category="Documentation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="880b770e-c1b1-4d40-913f-27da50f00a38" Description="Additional notes for this event property operation" Name="Notes" DisplayName="Notes" Category="Documentation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="ad2fdb44-d3a1-4329-8a09-bf704e473e05" Description="Description for CQRSAzure.CQRSdsl.Dsl.QueryReturnParameter" Name="QueryReturnParameter" DisplayName="Query Return Parameter" Namespace="CQRSAzure.CQRSdsl.Dsl">
      <Properties>
        <DomainProperty Id="54fb2a1b-8b49-4fa1-a1ea-eb450404d159" Description="The name of the returned parameter" Name="Name" DisplayName="Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="d252485c-a64f-45cd-840a-ac467214a761" Description="The business description of the returned parameter" Name="Description" DisplayName="Description" Category="Documentation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="d224d8cd-9b11-46ea-84f6-e7e8a81edcb4" Description="The base data type returned by the query" Name="DataType" DisplayName="Data Type">
          <Type>
            <DomainEnumerationMoniker Name="PropertyDataType" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="29e81073-c72c-49af-8cda-ab5dff9f9a8b" Description="Additional notes for the query return parameter" Name="Notes" DisplayName="Notes" Category="Documentation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="1dc6d5ad-aa3c-45e5-a876-a69d6e916ac5" Description="A setting that controls how the model is turned into code or documentation" Name="ModelSetting" DisplayName="Model Setting" Namespace="CQRSAzure.CQRSdsl.Dsl">
      <Properties>
        <DomainProperty Id="9542884f-3021-4604-9e52-b035f914f700" Description="The unique name of this model setting" Name="Name" DisplayName="Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="170d46e0-646c-4e90-b915-5665f9198024" Description="The value of this model setting" Name="Value" DisplayName="Value">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="9fa1697d-48e7-4b70-afc0-8eb600d6fb6e" Description="A collection of 0 or more aggregate identifiers that share a common attribute that denotes identity" Name="IdentityGroup" DisplayName="Identity Group" Namespace="CQRSAzure.CQRSdsl.Dsl">
      <Notes>Identity is an attribute that x is a y</Notes>
      <Properties>
        <DomainProperty Id="61a22c40-127d-433c-8217-6a49b06d4d82" Description="Does this identity group identify one and only one aggregate identifier" Name="IsInstance" DisplayName="Is Instance" DefaultValue="False" Category="Cardinality">
          <Notes>Does this identity group identify one and only one aggregate identifier</Notes>
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="43bbddb4-3366-4755-8171-a24468f4a662" Description="Does this identity group include ALL known aggregate identifiers" Name="IsGlobal" DisplayName="Is Global" DefaultValue="False" Category="Cardinality">
          <Notes>Does this group represent all known instances of the aggregate identifier?</Notes>
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="5ec842fc-a019-4ee4-9323-9f929a25dd68" Description="Unique name of the identity group" Name="Name" DisplayName="Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="fe769aae-a147-42f2-a5b4-80bae5fd0ce7" Description="The description of this identity group for documentation" Name="Description" DisplayName="Description" Category="Documentation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="558fa47e-1e5c-4aa3-8b47-b942a9ee0d38" Description="Additional documentation for the identity group" Name="Notes" DisplayName="Notes" Category="Documentation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="54acd334-2c3e-41da-9229-61f3da235d3e" Description="A tag /category that can be applied to an entity" Name="Category" DisplayName="Category">
          <Notes>This allows showing/hiding things by category (also known as onion-skinning)</Notes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="d7d66aea-3f0b-451f-9730-da862a4d5368" Description="A specialised projection which classifies entities as being either in or out of a particular identity group" Name="Classifier" DisplayName="Classifier" Namespace="CQRSAzure.CQRSdsl.Dsl">
      <Notes>Although the code generated is very similar to the standard projection, the differentiation by the name classifier is an important aid to understanding the model</Notes>
      <Properties>
        <DomainProperty Id="2ca07bb0-9e90-44d4-8ae9-1509bee8ff9b" Description="The unique name of the classifier" Name="Name" DisplayName="Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="8d344989-ab9c-48d7-89ff-dc8756f05264" Description="The description of the aggregate identity classifier" Name="Description" DisplayName="Description" Category="Documentation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b4d03fa1-ea49-4455-97fd-b24dbb1b60f2" Description="Additional notes for the aggregate identity classifier" Name="Notes" DisplayName="Notes" Category="Documentation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="4030096f-56f9-41e1-a1e4-10e988b12919" Description="A tag /category that can be applied to an entity" Name="Category" DisplayName="Category">
          <Notes>This allows showing/hiding things by category (also known as onion-skinning)</Notes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="ClassifierEventEvaluation" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>ClassifierHasEventEvaluations.ClassifierEventEvaluations</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="e2e7196c-d1c1-48bf-9e7f-ead7b718bbb9" Description="An evaluation to perform when an event is encountered to decide if an identity is or is not a member of an identity group" Name="ClassifierEventEvaluation" DisplayName="Classifier Event Evaluation" Namespace="CQRSAzure.CQRSdsl.Dsl">
      <Properties>
        <DomainProperty Id="f946a237-dd8b-4291-9bc4-e8d5343fc955" Description="The name of the event the classifier is handling" Name="EventName" DisplayName="Event Name">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(CQRSAzure.CQRSdsl.Dsl.CustomCode.UI.ClassifierEventEvaluationEventNameUITypeEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="22945350-583c-4a39-a869-a47674967660" Description="The property of the event that is to be evaluated" Name="SourceEventPropertyName" DisplayName="Source Event Property Name">
          <Notes>This can be blank if the evaluation operation is "Any" which always evaluates to true</Notes>
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(CQRSAzure.CQRSdsl.Dsl.CustomCode.UI.ClassifierEventSourceEventPropertyNameUITypeEditor )" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="eecfb060-6a83-48d1-b182-c4ca339712bc" Description="Additional notes pertaining to this classifier event evaluation" Name="Notes" DisplayName="Notes" Category="Documentation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b822fe64-6c98-480d-9dd2-e0f686cec868" Description="Description of this classifier event evaluation" Name="Description" DisplayName="Description">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="5e7d6453-ab52-444f-8f53-8807f2d27311" Description="The evaluation to perform on the event property when the event is handled" Name="PropertyEvaluationToPerform" DisplayName="Property Evaluation To Perform" DefaultValue="Always">
          <Type>
            <DomainEnumerationMoniker Name="PropertyEvaluation" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="cbf63e04-7bcb-4184-a1ed-7131cc889413" Description="What the property evaluating to true means to the identity group" Name="OnTrue" DisplayName="On True" DefaultValue="Include">
          <Type>
            <DomainEnumerationMoniker Name="IdentityGroupClassification" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="8c4a0eb9-a64f-48a0-b4b4-433a92a4eb18" Description="What happens to group membership if this property evaluates to false" Name="OnFalse" DisplayName="On False" DefaultValue="Exclude">
          <Type>
            <DomainEnumerationMoniker Name="IdentityGroupClassification" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="9aa691ae-91db-4703-a3db-15199b5366ac" Description="What the property value is being evaluated against" Name="Target" DisplayName="Target">
          <Notes>If the target type is a constant then this is the constant's value, otherwise the variable name</Notes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="580bb78e-2259-430c-bcf8-692ab24d16d1" Description="What is represented by the evaluation target" Name="TargetType" DisplayName="Target Type" DefaultValue="Constant">
          <Type>
            <DomainEnumerationMoniker Name="EvaluationTargetType" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
  </Classes>
  <Relationships>
    <DomainRelationship Id="47895ddd-8bf3-4b75-ac66-588d2faebea4" Description="Aggregate identifiers (types) in this domain" Name="CQRSModelHasAggregateIdentifiers" DisplayName="CQRSModel has Aggregate Identifiers" Namespace="CQRSAzure.CQRSdsl.Dsl" IsEmbedding="true">
      <Notes>The set of top level aggregates (also refered to as entities) in the model to which events can occur</Notes>
      <Source>
        <DomainRole Id="52a788dc-6b6b-4f5b-a35a-b8c83fcbb2d3" Description="Description for CQRSAzure.CQRSdsl.Dsl.CQRSModelHasAggregateIdentifiers.CQRSModel" Name="CQRSModel" DisplayName="CQRSModel" PropertyName="AggregateIdentifiers" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Aggregate Identifiers">
          <RolePlayer>
            <DomainClassMoniker Name="CQRSModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="b68fbfb5-508c-4bbd-8da1-c1daaa5bc804" Description="Description for CQRSAzure.CQRSdsl.Dsl.CQRSModelHasAggregateIdentifiers.AggregateIdentifier" Name="AggregateIdentifier" DisplayName="Aggregate Identifier" PropertyName="CQRSModel" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="CQRSModel">
          <RolePlayer>
            <DomainClassMoniker Name="AggregateIdentifier" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="e2674e6c-4bfd-4a31-9ab9-7c0a1753c011" Description="Description for CQRSAzure.CQRSdsl.Dsl.AggregateIdentifierHasEventDefinitions" Name="AggregateIdentifierHasEventDefinitions" DisplayName="Aggregate Identifier Has Event Definitions" Namespace="CQRSAzure.CQRSdsl.Dsl" IsEmbedding="true">
      <Properties>
        <DomainProperty Id="c0f79858-8b69-4dfd-a770-09975a35ac00" Description="The name of the link between the aggregate and its events" Name="Name" DisplayName="Relationship Name" IsElementName="true">
          <Notes>This name is used to find the linkage in the property browser</Notes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="87dbd11a-f809-4013-97eb-6187d0065337" Description="Description for CQRSAzure.CQRSdsl.Dsl.AggregateIdentifierHasEventDefinitions.AggregateIdentifier" Name="AggregateIdentifier" DisplayName="Aggregate Identifier" PropertyName="EventDefinitions" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Event Definitions">
          <RolePlayer>
            <DomainClassMoniker Name="AggregateIdentifier" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="9a5ecfa5-27ee-4faf-9e9d-0a528850654e" Description="Description for CQRSAzure.CQRSdsl.Dsl.AggregateIdentifierHasEventDefinitions.EventDefinition" Name="EventDefinition" DisplayName="Event Definition" PropertyName="AggregateIdentifier" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Aggregate Identifier">
          <RolePlayer>
            <DomainClassMoniker Name="EventDefinition" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="ba3dd482-7ba0-49c9-9ab7-5364bb64302b" Description="Description for CQRSAzure.CQRSdsl.Dsl.AggregateIdentifierHasProjectionDefinitions" Name="AggregateIdentifierHasProjectionDefinitions" DisplayName="Aggregate Identifier Has Projection Definitions" Namespace="CQRSAzure.CQRSdsl.Dsl" IsEmbedding="true">
      <Properties>
        <DomainProperty Id="2509c202-5f4e-4892-af54-fedbb37dd46c" Description="The name of the link between the aggregate and its projections" Name="Name" DisplayName="Relationship Name" IsElementName="true">
          <Notes>This is mainly used to find the link in the property browser</Notes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="f57074c2-5fd1-47a1-b27c-adc651344e53" Description="Description for CQRSAzure.CQRSdsl.Dsl.AggregateIdentifierHasProjectionDefinitions.AggregateIdentifier" Name="AggregateIdentifier" DisplayName="Aggregate Identifier" PropertyName="ProjectionDefinitions" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Projection Definitions">
          <RolePlayer>
            <DomainClassMoniker Name="AggregateIdentifier" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="5d70ec76-f730-442c-901c-ae279d2cc243" Description="Description for CQRSAzure.CQRSdsl.Dsl.AggregateIdentifierHasProjectionDefinitions.ProjectionDefinition" Name="ProjectionDefinition" DisplayName="Projection Definition" PropertyName="AggregateIdentifier" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Aggregate Identifier">
          <RolePlayer>
            <DomainClassMoniker Name="ProjectionDefinition" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="4a0b0281-9475-4589-843c-981ed16dec78" Description="Relationship between an aggregate identifier and its parent identifier (if such exists)" Name="AggregateIdentifierIsChildOfTargetAggregateIdentifiers" DisplayName="Aggregate Identifier Is Child Of Target Aggregate Identifiers" Namespace="CQRSAzure.CQRSdsl.Dsl">
      <Notes>Aggregates can be arranged hierarchically in a system (although this is not required)</Notes>
      <Properties>
        <DomainProperty Id="bc27cfb9-eb12-4f97-be03-9e3d5ef229fd" Description="The name of the parent-child relationship between aggregations" Name="Name" DisplayName="Relationship Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="ce8a1056-effd-4b73-b3ce-b465f9a045cd" Description="Description for CQRSAzure.CQRSdsl.Dsl.AggregateIdentifierIsChildOfTargetAggregateIdentifiers.SourceAggregateIdentifier" Name="SourceAggregateIdentifier" DisplayName="Source Aggregate Identifier" PropertyName="TargetAggregateIdentifiers" PropertyDisplayName="Target Aggregate Identifiers">
          <RolePlayer>
            <DomainClassMoniker Name="AggregateIdentifier" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="de708f59-e445-4b30-b91a-2e5e98a79213" Description="Description for CQRSAzure.CQRSdsl.Dsl.AggregateIdentifierIsChildOfTargetAggregateIdentifiers.TargetAggregateIdentifier" Name="TargetAggregateIdentifier" DisplayName="Target Aggregate Identifier" PropertyName="SourceAggregateIdentifiers" PropertyDisplayName="Source Aggregate Identifiers">
          <RolePlayer>
            <DomainClassMoniker Name="AggregateIdentifier" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="95cb214e-1625-4929-9f3f-e9a22f926615" Description="Description for CQRSAzure.CQRSdsl.Dsl.AggregateIdentifierHasCommandDefinitions" Name="AggregateIdentifierHasCommandDefinitions" DisplayName="Aggregate Identifier Has Command Definitions" Namespace="CQRSAzure.CQRSdsl.Dsl" IsEmbedding="true">
      <Properties>
        <DomainProperty Id="3aed6b89-d9cc-4edf-bdb0-734503aa9adc" Description="The name of the connection between an aggregate and its command definitions" Name="Name" DisplayName="Relationship Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="ea4fdf99-c277-4b52-bf75-d506227ea40b" Description="Description for CQRSAzure.CQRSdsl.Dsl.AggregateIdentifierHasCommandDefinitions.AggregateIdentifier" Name="AggregateIdentifier" DisplayName="Aggregate Identifier" PropertyName="CommandDefinitions" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Command Definitions">
          <RolePlayer>
            <DomainClassMoniker Name="AggregateIdentifier" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="1ade1c6b-a107-4e4f-abb5-44e43f834dc3" Description="Description for CQRSAzure.CQRSdsl.Dsl.AggregateIdentifierHasCommandDefinitions.CommandDefinition" Name="CommandDefinition" DisplayName="Command Definition" PropertyName="AggregateIdentifier" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Aggregate Identifier">
          <RolePlayer>
            <DomainClassMoniker Name="CommandDefinition" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="1ee9384a-cf1d-4dd5-9957-cc118deeefd7" Description="Description for CQRSAzure.CQRSdsl.Dsl.EventDefinitionHasEventProperties" Name="EventDefinitionHasEventProperties" DisplayName="Event Definition Has Event Properties" Namespace="CQRSAzure.CQRSdsl.Dsl" IsEmbedding="true">
      <Properties>
        <DomainProperty Id="6a1d6427-d4f2-4092-a9f0-f7e01d482926" Description="The version of the event definition for which this property was first added" Name="CreatedVersion" DisplayName="Created Version" DefaultValue="0" Category="Versioning">
          <Type>
            <ExternalTypeMoniker Name="/System/UInt32" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e814cc9b-4f43-4029-a4b5-5f3f8997db31" Description="The version number after which this property is no longer implemented" Name="DepreciatedVersion" DisplayName="Depreciated Version" DefaultValue="0">
          <Notes>If this value is zero the property is not depreciated.  </Notes>
          <Type>
            <ExternalTypeMoniker Name="/System/UInt32" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="2c4fb3b0-c9b3-44bc-aec8-65be7d976a56" Description="The name of the link between the event and its properties" Name="Name" DisplayName="Relationship Name">
          <Notes>This is mainly for navigation</Notes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="dfd27afd-8260-497a-a4a7-ad67d0dd81e2" Description="Description for CQRSAzure.CQRSdsl.Dsl.EventDefinitionHasEventProperties.EventDefinition" Name="EventDefinition" DisplayName="Event Definition" PropertyName="EventProperties" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Event Properties">
          <RolePlayer>
            <DomainClassMoniker Name="EventDefinition" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="b7a944e4-a15b-4d60-9a47-0260307bccf1" Description="Description for CQRSAzure.CQRSdsl.Dsl.EventDefinitionHasEventProperties.EventProperty" Name="EventProperty" DisplayName="Event Property" PropertyName="EventDefinition" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Event Definition">
          <RolePlayer>
            <DomainClassMoniker Name="EventProperty" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="22b7c8f3-e336-46b2-9266-a633d5cda505" Description="Description for CQRSAzure.CQRSdsl.Dsl.ProjectionDefinitionHasProjectionProperties" Name="ProjectionDefinitionHasProjectionProperties" DisplayName="Projection Definition Has Projection Properties" Namespace="CQRSAzure.CQRSdsl.Dsl" IsEmbedding="true">
      <Properties>
        <DomainProperty Id="6e235a26-0a9e-4ae3-bf70-0d6aadc44376" Description="The name of the connection between the projection and its properties" Name="Name" DisplayName="Relationship Name" IsElementName="true">
          <Notes>This is mainly used to make navigation easier</Notes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="693c2022-95a2-495e-aa33-8c86113d0888" Description="Description for CQRSAzure.CQRSdsl.Dsl.ProjectionDefinitionHasProjectionProperties.ProjectionDefinition" Name="ProjectionDefinition" DisplayName="Projection Definition" PropertyName="ProjectionProperties" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Projection Properties">
          <RolePlayer>
            <DomainClassMoniker Name="ProjectionDefinition" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="63f6afa3-fa12-4fe0-b5d0-8c31a9ad6881" Description="Description for CQRSAzure.CQRSdsl.Dsl.ProjectionDefinitionHasProjectionProperties.ProjectionProperty" Name="ProjectionProperty" DisplayName="Projection Property" PropertyName="ProjectionDefinition" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Projection Definition">
          <RolePlayer>
            <DomainClassMoniker Name="ProjectionProperty" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="0d9211f3-f871-40db-b147-8ed562f84e1a" Description="Description for CQRSAzure.CQRSdsl.Dsl.CommandDefinitionHasParameters" Name="CommandDefinitionHasParameters" DisplayName="Command Definition Has Parameters" Namespace="CQRSAzure.CQRSdsl.Dsl" IsEmbedding="true">
      <Properties>
        <DomainProperty Id="bec9031d-d613-48df-98ca-ff435fdb04d3" Description="The name of the connection between the command definition and its parameters" Name="Name" DisplayName="Relationship Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="a118b289-62b6-4a80-afaf-11a538971b93" Description="Description for CQRSAzure.CQRSdsl.Dsl.CommandDefinitionHasParameters.CommandDefinition" Name="CommandDefinition" DisplayName="Command Definition" PropertyName="CommandParameters" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Command Parameters">
          <RolePlayer>
            <DomainClassMoniker Name="CommandDefinition" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="6b1a28b9-3e0c-4ef3-9386-3f6d8b4684bc" Description="Description for CQRSAzure.CQRSdsl.Dsl.CommandDefinitionHasParameters.CommandParameter" Name="CommandParameter" DisplayName="Command Parameter" PropertyName="CommandDefinition" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Command Definition">
          <RolePlayer>
            <DomainClassMoniker Name="CommandParameter" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="da104a14-17c3-4cfa-960d-9bea21de7f05" Description="Description for CQRSAzure.CQRSdsl.Dsl.AggregateIdentifierHasQueryDefinitions" Name="AggregateIdentifierHasQueryDefinitions" DisplayName="Aggregate Identifier Has Query Definitions" Namespace="CQRSAzure.CQRSdsl.Dsl" IsEmbedding="true">
      <Properties>
        <DomainProperty Id="eb90ac02-07d2-42d8-bcb6-7de56fd43dcb" Description="The name of the connection between an aggregate and its query definitions" Name="Name" DisplayName="Relationship Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="ae82de13-4c00-4873-addf-bb63be3b0b2a" Description="Description for CQRSAzure.CQRSdsl.Dsl.AggregateIdentifierHasQueryDefinitions.AggregateIdentifier" Name="AggregateIdentifier" DisplayName="Aggregate Identifier" PropertyName="QueryDefinitions" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Query Definitions">
          <RolePlayer>
            <DomainClassMoniker Name="AggregateIdentifier" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="ac0977e5-b342-4a33-9746-1a907db6a51b" Description="Description for CQRSAzure.CQRSdsl.Dsl.AggregateIdentifierHasQueryDefinitions.QueryDefinition" Name="QueryDefinition" DisplayName="Query Definition" PropertyName="AggregateIdentifier" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Aggregate Identifier">
          <RolePlayer>
            <DomainClassMoniker Name="QueryDefinition" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="dd524b21-7c58-4b30-a106-e95afb35289d" Description="Description for CQRSAzure.CQRSdsl.Dsl.QueryDefinitionHasQueryInputParameters" Name="QueryDefinitionHasQueryInputParameters" DisplayName="Query Definition Has Query Input Parameters" Namespace="CQRSAzure.CQRSdsl.Dsl" IsEmbedding="true">
      <Properties>
        <DomainProperty Id="c28f5af0-c251-4229-8421-bdca22bfcd3e" Description="The name of the link between a query definition and its parameters" Name="Name" DisplayName="Relationship Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="ed43ead9-02d3-46f3-9687-67b2738d77ea" Description="Description for CQRSAzure.CQRSdsl.Dsl.QueryDefinitionHasQueryInputParameters.QueryDefinition" Name="QueryDefinition" DisplayName="Query Definition" PropertyName="QueryInputParameters" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Query Input Parameters">
          <RolePlayer>
            <DomainClassMoniker Name="QueryDefinition" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="466014d9-b275-4287-b136-7e85baea5084" Description="Description for CQRSAzure.CQRSdsl.Dsl.QueryDefinitionHasQueryInputParameters.QueryInputParameter" Name="QueryInputParameter" DisplayName="Query Input Parameter" PropertyName="QueryDefinition" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Query Definition">
          <RolePlayer>
            <DomainClassMoniker Name="QueryInputParameter" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="60025a34-777a-44f2-af11-bfcf9126a4a4" Description="Link to show that a projection handles the given named event" Name="ProjectionDefinitionHandlesEventDefinitions" DisplayName="Projection Definition Handles Event Definitions" Namespace="CQRSAzure.CQRSdsl.Dsl">
      <Notes>Specifies that a projection performs some logic when it handles the given event</Notes>
      <Properties>
        <DomainProperty Id="539dfade-4006-4584-9c67-26f31b0a6467" Description="The name of the link between the projection and the events it handles" Name="Name" DisplayName="Relationship Name" IsElementName="true">
          <Notes>This is mainly for navigation</Notes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="7b7d9bbc-5141-4619-bd30-7b882e634025" Description="Description for CQRSAzure.CQRSdsl.Dsl.ProjectionDefinitionHandlesEventDefinitions.ProjectionDefinition" Name="ProjectionDefinition" DisplayName="Projection Definition" PropertyName="EventDefinitions" PropertyDisplayName="Event Definitions">
          <RolePlayer>
            <DomainClassMoniker Name="ProjectionDefinition" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="546e31e8-08d8-4d32-920c-64b59a459a12" Description="Description for CQRSAzure.CQRSdsl.Dsl.ProjectionDefinitionHandlesEventDefinitions.EventDefinition" Name="EventDefinition" DisplayName="Event Definition" PropertyName="ProjectionDefinitions" PropertyDisplayName="Projection Definitions">
          <RolePlayer>
            <DomainClassMoniker Name="EventDefinition" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="e57baa58-91fd-4bc1-a993-eabe9e841b29" Description="The operations performed on the projection's properties when an event is handled" Name="ProjectionDefinitionHasEventPropertyOperations" DisplayName="Projection Definition Has Event Property Operations" Namespace="CQRSAzure.CQRSdsl.Dsl" GeneratesDoubleDerived="true" IsEmbedding="true">
      <Properties>
        <DomainProperty Id="78178afe-2067-4fc4-ab3a-2a7d67ceff64" Description="The name of the connection between the projection and its event property operations" Name="Name" DisplayName="Relationship Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="3d5a2934-3d1e-44bc-a59c-3f289d76cbaf" Description="Description for CQRSAzure.CQRSdsl.Dsl.ProjectionDefinitionHasEventPropertyOperations.ProjectionDefinition" Name="ProjectionDefinition" DisplayName="Projection Definition" PropertyName="ProjectionEventPropertyOperations" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Projection Event Property Operations">
          <RolePlayer>
            <DomainClassMoniker Name="ProjectionDefinition" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="24013117-68da-482c-968b-f8cb72022ff7" Description="Description for CQRSAzure.CQRSdsl.Dsl.ProjectionDefinitionHasEventPropertyOperations.ProjectionEventPropertyOperation" Name="ProjectionEventPropertyOperation" DisplayName="Projection Event Property Operation" PropertyName="ProjectionDefinition" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Projection Definition">
          <RolePlayer>
            <DomainClassMoniker Name="ProjectionEventPropertyOperation" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="2d8626d1-f266-49e5-a515-4bf8a6a4b1e0" Description="Description for CQRSAzure.CQRSdsl.Dsl.QueryDefinitionHasQueryReturnParameters" Name="QueryDefinitionHasQueryReturnParameters" DisplayName="Query Definition Has Query Return Parameters" Namespace="CQRSAzure.CQRSdsl.Dsl" IsEmbedding="true">
      <Properties>
        <DomainProperty Id="5bf3bcc4-0066-41a5-a13b-b2513522e262" Description="The name of the link between a query definition and its return parameters" Name="Name" DisplayName="Relationship Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="bb05587e-0204-484e-b726-0b03a7943c34" Description="Description for CQRSAzure.CQRSdsl.Dsl.QueryDefinitionHasQueryReturnParameters.QueryDefinition" Name="QueryDefinition" DisplayName="Query Definition" PropertyName="QueryReturnParameters" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Query Return Parameters">
          <RolePlayer>
            <DomainClassMoniker Name="QueryDefinition" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="bd42eddf-2db8-4269-9fd0-670915b6cc67" Description="Description for CQRSAzure.CQRSdsl.Dsl.QueryDefinitionHasQueryReturnParameters.QueryReturnParameter" Name="QueryReturnParameter" DisplayName="Query Return Parameter" PropertyName="QueryDefinition" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Query Definition">
          <RolePlayer>
            <DomainClassMoniker Name="QueryReturnParameter" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="2a88a228-fa99-41c6-8bb8-fb443b96ce5e" Description="Additional setting that controsl how the model is truend into code or documentation" Name="CQRSModelHasModelSet" DisplayName="CQRSModel Has Model Set" Namespace="CQRSAzure.CQRSdsl.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="b7cc451c-4223-43ac-8ffd-684cd0571bb2" Description="Description for CQRSAzure.CQRSdsl.Dsl.CQRSModelHasModelSet.CQRSModel" Name="CQRSModel" DisplayName="CQRSModel" PropertyName="ModelSet" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Model Set">
          <RolePlayer>
            <DomainClassMoniker Name="CQRSModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="685143c1-54fd-4a30-88dc-d825c4e53533" Description="Description for CQRSAzure.CQRSdsl.Dsl.CQRSModelHasModelSet.ModelSetting" Name="ModelSetting" DisplayName="Model Setting" PropertyName="CQRSModel" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="CQRSModel">
          <RolePlayer>
            <DomainClassMoniker Name="ModelSetting" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="c43057ea-90f5-4c21-a802-5d9bf98cedff" Description="The identity reference group to use to run this query against a set of aggregate identifiers" Name="QueryDefinitionReferencesIdentityGroup" DisplayName="Query Definition References Identity Group" Namespace="CQRSAzure.CQRSdsl.Dsl">
      <Source>
        <DomainRole Id="551bda20-afe0-4f4e-9200-a2bed0dd0cac" Description="Description for CQRSAzure.CQRSdsl.Dsl.QueryDefinitionReferencesIdentityGroup.QueryDefinition" Name="QueryDefinition" DisplayName="Query Definition" PropertyName="IdentityGroup" Multiplicity="ZeroOne" PropertyDisplayName="Identity Group">
          <RolePlayer>
            <DomainClassMoniker Name="QueryDefinition" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="f11c2a82-1c3f-4ef9-944b-9c4418b90db1" Description="The identity group referenced by this query definition" Name="IdentityGroup" DisplayName="Identity Group" PropertyName="QueryDefinitions" PropertyDisplayName="Query Definitions">
          <RolePlayer>
            <DomainClassMoniker Name="IdentityGroup" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="54070090-e75a-4fe5-b237-02bd51ca3089" Description="Description for CQRSAzure.CQRSdsl.Dsl.AggregateIdentifierHasIdentityGrouped" Name="AggregateIdentifierHasIdentityGrouped" DisplayName="Aggregate Identifier Has Identity Grouped" Namespace="CQRSAzure.CQRSdsl.Dsl" IsEmbedding="true">
      <Properties>
        <DomainProperty Id="6e3007ab-4939-4e5d-a0ab-dde2853e6abb" Description="The name of the conenction between an aggregate and identity group" Name="Name" DisplayName="Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="f489c4fe-4858-4ace-ae94-07d3b8125e10" Description="Description for CQRSAzure.CQRSdsl.Dsl.AggregateIdentifierHasIdentityGrouped.AggregateIdentifier" Name="AggregateIdentifier" DisplayName="Aggregate Identifier" PropertyName="IdentityGrouped" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Identity Grouped">
          <RolePlayer>
            <DomainClassMoniker Name="AggregateIdentifier" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="a9d5b1cd-2477-490e-ae92-6d764e9dff64" Description="Description for CQRSAzure.CQRSdsl.Dsl.AggregateIdentifierHasIdentityGrouped.IdentityGroup" Name="IdentityGroup" DisplayName="Identity Group" PropertyName="AggregateIdentifier" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Aggregate Identifier">
          <RolePlayer>
            <DomainClassMoniker Name="IdentityGroup" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="3451bf45-5f57-4be0-aeeb-8c431da8f175" Description="Description for CQRSAzure.CQRSdsl.Dsl.AggregateIdentifierHasClassifiers" Name="AggregateIdentifierHasClassifiers" DisplayName="Aggregate Identifier Has Classifiers" Namespace="CQRSAzure.CQRSdsl.Dsl" IsEmbedding="true">
      <Properties>
        <DomainProperty Id="f1ad9902-64ba-42ca-9b49-ae0129d93604" Description="The name of the connection between an aggregate and its classifiers" Name="Name" DisplayName="Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="47842c1f-9515-4761-b164-5c5821e8bfac" Description="Description for CQRSAzure.CQRSdsl.Dsl.AggregateIdentifierHasClassifiers.AggregateIdentifier" Name="AggregateIdentifier" DisplayName="Aggregate Identifier" PropertyName="Classifiers" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Classifiers">
          <RolePlayer>
            <DomainClassMoniker Name="AggregateIdentifier" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="bfad1fc4-b0d8-4d6d-adc9-76ef948ed528" Description="Description for CQRSAzure.CQRSdsl.Dsl.AggregateIdentifierHasClassifiers.Classifier" Name="Classifier" DisplayName="Classifier" PropertyName="AggregateIdentifier" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Aggregate Identifier">
          <RolePlayer>
            <DomainClassMoniker Name="Classifier" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="fa8f6c71-9982-4767-b894-211219e18044" Description="The classifier class that creates the membership of an identity group" Name="IdentityGroupReferencesClassifier" DisplayName="Identity Group References Classifier" Namespace="CQRSAzure.CQRSdsl.Dsl">
      <Properties>
        <DomainProperty Id="3b804a9b-c201-43ff-874a-4189a17200bf" Description="Description for CQRSAzure.CQRSdsl.Dsl.IdentityGroupReferencesClassifier.Name" Name="Name" DisplayName="Name">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="0cc9a7fb-5bdf-40a0-9d1c-dd30bfca0cd5" Description="Description for CQRSAzure.CQRSdsl.Dsl.IdentityGroupReferencesClassifier.IdentityGroup" Name="IdentityGroup" DisplayName="Identity Group" PropertyName="Classifier" Multiplicity="ZeroOne" PropertyDisplayName="Classifier">
          <RolePlayer>
            <DomainClassMoniker Name="IdentityGroup" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="084b7e27-ab06-44b5-9dff-084992ce2ac0" Description="Description for CQRSAzure.CQRSdsl.Dsl.IdentityGroupReferencesClassifier.Classifier" Name="Classifier" DisplayName="Classifier" PropertyName="IdentityGroup" Multiplicity="One" PropertyDisplayName="Identity Group">
          <RolePlayer>
            <DomainClassMoniker Name="Classifier" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="c25a88db-73f9-492f-ba88-2cb559e4a080" Description="The events that are processed by the classifier when deciding if an identifier is in our out of the group" Name="ClassifierHandlesEvents" DisplayName="Classifier Handles Events" Namespace="CQRSAzure.CQRSdsl.Dsl">
      <Notes>A classifier will respond to handled eventys to decide if an identity is in or out of the identity group</Notes>
      <Source>
        <DomainRole Id="546a6882-20f1-4b39-9205-69355c6f3e9a" Description="Description for CQRSAzure.CQRSdsl.Dsl.ClassifierHandlesEvents.Classifier" Name="Classifier" DisplayName="Classifier" PropertyName="EventDefinitions" PropertyDisplayName="Event Definitions">
          <RolePlayer>
            <DomainClassMoniker Name="Classifier" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="1cdd86b8-4a06-4213-9f59-f385d32cf34b" Description="Description for CQRSAzure.CQRSdsl.Dsl.ClassifierHandlesEvents.EventDefinition" Name="EventDefinition" DisplayName="Event Definition" PropertyName="Classifiers" PropertyDisplayName="Classifiers">
          <RolePlayer>
            <DomainClassMoniker Name="EventDefinition" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="9bf453e0-7df3-4c6c-a815-3b8b977d341c" Description="The evaluations to be performed by the classifier" Name="ClassifierHasEventEvaluations" DisplayName="Classifier Has Event Evaluations" Namespace="CQRSAzure.CQRSdsl.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="7e606fca-2e25-4360-8c22-604b0907203d" Description="Description for CQRSAzure.CQRSdsl.Dsl.ClassifierHasEventEvaluations.Classifier" Name="Classifier" DisplayName="Classifier" PropertyName="ClassifierEventEvaluations" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Classifier Event Evaluations">
          <RolePlayer>
            <DomainClassMoniker Name="Classifier" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="ee628483-3aec-4dcf-9418-e1f4670d4ba4" Description="Description for CQRSAzure.CQRSdsl.Dsl.ClassifierHasEventEvaluations.ClassifierEventEvaluation" Name="ClassifierEventEvaluation" DisplayName="Classifier Event Evaluation" PropertyName="Classifier" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Classifier">
          <RolePlayer>
            <DomainClassMoniker Name="ClassifierEventEvaluation" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="26a7f252-842b-48df-aa02-c24a73958c86" Description="Description for CQRSAzure.CQRSdsl.Dsl.CommandDefinitionReferencesIdentityGroup" Name="CommandDefinitionReferencesIdentityGroup" DisplayName="Command Definition References Identity Group" Namespace="CQRSAzure.CQRSdsl.Dsl">
      <Source>
        <DomainRole Id="9f111275-39d2-4ef6-8ed4-772b818cd137" Description="Description for CQRSAzure.CQRSdsl.Dsl.CommandDefinitionReferencesIdentityGroup.CommandDefinition" Name="CommandDefinition" DisplayName="Command Definition" PropertyName="IdentityGroup" Multiplicity="ZeroOne" PropertyDisplayName="Identity Group">
          <RolePlayer>
            <DomainClassMoniker Name="CommandDefinition" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="cb198eea-18c2-4b61-b4f5-436810c917ae" Description="Description for CQRSAzure.CQRSdsl.Dsl.CommandDefinitionReferencesIdentityGroup.IdentityGroup" Name="IdentityGroup" DisplayName="Identity Group" PropertyName="CommandDefinitions" PropertyDisplayName="Command Definitions">
          <RolePlayer>
            <DomainClassMoniker Name="IdentityGroup" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="ab39163b-1c2b-46e8-81d3-d4c0ae2ed538" Description="Conenction from a query to the projection underlying the query results" Name="QueryDefinitionReferencesProjectionDefinition" DisplayName="Query Definition References Projection Definition" Namespace="CQRSAzure.CQRSdsl.Dsl">
      <Notes>The projection underlying this query</Notes>
      <Source>
        <DomainRole Id="2a6c36b4-eed2-409c-b669-d4fce7ae1227" Description="Description for CQRSAzure.CQRSdsl.Dsl.QueryDefinitionReferencesProjectionDefinition.QueryDefinition" Name="QueryDefinition" DisplayName="Query Definition" PropertyName="ProjectionDefinition" Multiplicity="ZeroOne" PropertyDisplayName="Projection Definition">
          <RolePlayer>
            <DomainClassMoniker Name="QueryDefinition" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="43a282df-ebf5-4e51-9407-bc6abb48dd57" Description="Description for CQRSAzure.CQRSdsl.Dsl.QueryDefinitionReferencesProjectionDefinition.ProjectionDefinition" Name="ProjectionDefinition" DisplayName="Projection Definition" PropertyName="QueryDefinitions" PropertyDisplayName="Query Definitions">
          <RolePlayer>
            <DomainClassMoniker Name="ProjectionDefinition" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
  </Relationships>
  <Types>
    <ExternalType Name="DateTime" Namespace="System" />
    <ExternalType Name="String" Namespace="System" />
    <ExternalType Name="Int16" Namespace="System" />
    <ExternalType Name="Int32" Namespace="System" />
    <ExternalType Name="Int64" Namespace="System" />
    <ExternalType Name="UInt16" Namespace="System" />
    <ExternalType Name="UInt32" Namespace="System" />
    <ExternalType Name="UInt64" Namespace="System" />
    <ExternalType Name="SByte" Namespace="System" />
    <ExternalType Name="Byte" Namespace="System" />
    <ExternalType Name="Double" Namespace="System" />
    <ExternalType Name="Single" Namespace="System" />
    <ExternalType Name="Guid" Namespace="System" />
    <ExternalType Name="Boolean" Namespace="System" />
    <ExternalType Name="Char" Namespace="System" />
    <DomainEnumeration Name="KeyDataType" Namespace="CQRSAzure.CQRSdsl.Dsl" Description="The distinct data types that can be used to provide the key for any given aggregate">
      <Notes>This allows an aggregate to be uniquely identified by some business relevant unique number or name if appropriate</Notes>
      <Literals>
        <EnumerationLiteral Description="A system provided globally unique identifier" Name="SystemGUID" Value="0">
          <Notes>This is the default</Notes>
        </EnumerationLiteral>
        <EnumerationLiteral Description="A system provided incremental record number" Name="IncrementalNumber" Value="1">
          <Notes>This is used where the records only need to be uniquely identified within this domain</Notes>
        </EnumerationLiteral>
        <EnumerationLiteral Description="A unique string provided by the domain" Name="DomainUniqueString" Value="2">
          <Notes>This could be some meaningful unique identifier like ISBN, CUSIP, Vehicle registration etc. </Notes>
        </EnumerationLiteral>
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="PropertyDataType" Namespace="CQRSAzure.CQRSdsl.Dsl" Description="The subset of data types used for properties in this system">
      <Notes>Properties of events and projections are of one of these simple data types to make the domain as transparent as possible</Notes>
      <Literals>
        <EnumerationLiteral Description="A whole number" Name="Integer" Value="0">
          <Notes>A whole number that can be zero or negative</Notes>
        </EnumerationLiteral>
        <EnumerationLiteral Description="A decimal number for currency / amounts" Name="Decimal" Value="1" />
        <EnumerationLiteral Description="A floating point number " Name="FloatingPointNumber" Value="2">
          <Notes>This is stored to the maximum precision available on the target system</Notes>
        </EnumerationLiteral>
        <EnumerationLiteral Description="A date and time" Name="Date" Value="3">
          <Notes>This is stored as a UTC date/time and converted to local time by any clients</Notes>
        </EnumerationLiteral>
        <EnumerationLiteral Description="A string of text" Name="String" Value="4">
          <Notes>No maximum length defined here but implementations may impose their own maximum lengths</Notes>
        </EnumerationLiteral>
        <EnumerationLiteral Description="A picture or image" Name="Image" Value="5">
          <Notes>This will be stored in whatever format (jpeg, bmp, etc. is chosen by the implementation layer</Notes>
        </EnumerationLiteral>
        <EnumerationLiteral Description="A true or false value" Name="Boolean" Value="6" />
        <EnumerationLiteral Description="Globally unique identifier" Name="GUID" Value="7">
          <Notes>A globally unique identifier</Notes>
        </EnumerationLiteral>
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="PropertyOperation" Namespace="CQRSAzure.CQRSdsl.Dsl" Description="What is done to a property when the event is handled">
      <Notes>This allows us to specify basic operations to perform when handling an event.  Most projections will be based off these but more advanced functionality can be built in code.</Notes>
      <Literals>
        <EnumerationLiteral Description="Set the projection property value to the event named property" Name="SetToValue" Value="0">
          <Notes>Set the projection property value to the event named property.  Data type conversion can be performed.</Notes>
        </EnumerationLiteral>
        <EnumerationLiteral Description="Increment the property as a counter" Name="IncrementCount" Value="1" />
        <EnumerationLiteral Description="Decrement the property as a counter" Name="DecrementCount" Value="2" />
        <EnumerationLiteral Description="Increment the property by the value of an event's named property" Name="IncrementByValue" Value="3" />
        <EnumerationLiteral Description="Decrement the property by the value of an event's named property" Name="DecrementByValue" Value="4" />
        <EnumerationLiteral Description="Set a flag status property" Name="SetFlag" Value="5">
          <Notes>This sets a boolean value to true or a string to a defined value</Notes>
        </EnumerationLiteral>
        <EnumerationLiteral Description="Unset a flag status property" Name="UnsetFlag" Value="6">
          <Notes>Sets a boolean to false or unsets a text flag</Notes>
        </EnumerationLiteral>
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="TargetLanguage" Namespace="CQRSAzure.CQRSdsl.Dsl" Description="The language to generate the code in">
      <Literals>
        <EnumerationLiteral Description="C Sharp code output" Name="CSharp" Value="1" />
        <EnumerationLiteral Description="Output code in VB.Net" Name="VBNet" Value="2" />
      </Literals>
    </DomainEnumeration>
    <ExternalType Name="Color" Namespace="System.Drawing" />
    <DomainEnumeration Name="IdentityGroupClassification" Namespace="CQRSAzure.CQRSdsl.Dsl" Description="Does this include or exclude the entity from the identity group">
      <Literals>
        <EnumerationLiteral Description="Exclude the identity from the group (default)" Name="Exclude" Value="0" />
        <EnumerationLiteral Description="Include the identity in the group" Name="Include" Value="1" />
        <EnumerationLiteral Description="The membership (or not) is as it was before the event was handled" Name="Unchanged" Value="2">
          <Notes>This allows an event handler to only change the membership state if a particular property evaluation occurs</Notes>
        </EnumerationLiteral>
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="PropertyEvaluation" Namespace="CQRSAzure.CQRSdsl.Dsl" Description="An evaluation to perform on a source property of an event">
      <Literals>
        <EnumerationLiteral Description="The source property is empty/null" Name="IsEmpty" Value="9" />
        <EnumerationLiteral Description="The property equals the test value" Name="Equals" Value="1" />
        <EnumerationLiteral Description="Property is less than the value it is being compared to" Name="IsLessThan" Value="2" />
        <EnumerationLiteral Description="Property value is less than or equal to the comparison value" Name="IsLessThanOrEqualTo" Value="3" />
        <EnumerationLiteral Description="Property value is greater than the value it is being compared against" Name="IsGreaterThan" Value="4" />
        <EnumerationLiteral Description="The property value is greater than or equal to the value it is being compared against" Name="IsGreaterThanOrEqualTo" Value="5" />
        <EnumerationLiteral Description="The property value contains the test value" Name="Contains" Value="6">
          <Notes>This is only meaningful for string based properties</Notes>
        </EnumerationLiteral>
        <EnumerationLiteral Description="The property value starts with the test value" Name="StartsWith" Value="7">
          <Notes>This is only meaningful for string based properties</Notes>
        </EnumerationLiteral>
        <EnumerationLiteral Description="The property value ends with the test value" Name="EndsWith" Value="8">
          <Notes>This is only meaningful for string based properties</Notes>
        </EnumerationLiteral>
        <EnumerationLiteral Description="Always evaluates to TRUE" Name="Always" Value="0">
          <Notes>This is used when just the fact of the event is enough to trigger membership or otherwise of an identity group</Notes>
        </EnumerationLiteral>
        <EnumerationLiteral Description="A custom event evaluator" Name="Custom" Value="10">
          <Notes>A complex evaluator that will need customised coding</Notes>
        </EnumerationLiteral>
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="EvaluationTargetType" Namespace="CQRSAzure.CQRSdsl.Dsl" Description="What type of thing is the target of the property evaluation">
      <Literals>
        <EnumerationLiteral Description="The target is a hard-coded constant" Name="Constant" Value="0" />
        <EnumerationLiteral Description="The target is a named variable in the classifier class" Name="Variable" Value="1">
          <Notes>How the variable gets set and potentially changed is up to the implementer</Notes>
        </EnumerationLiteral>
      </Literals>
    </DomainEnumeration>
  </Types>
  <Shapes>
    <GeometryShape Id="77abb980-fc6e-4f88-82e2-e9d51e4a5834" Description="Shape used to denote an aggregate on the layoud diagram" Name="AggregateGeometryShape" DisplayName="Aggregate Geometry Shape" HelpKeyword="Aggregate" Namespace="CQRSAzure.CQRSdsl.Dsl" GeneratesDoubleDerived="true" TooltipType="Variable" FixedTooltipText="Aggregate Shape" FillColor="PaleGoldenrod" InitialHeight="1" OutlineThickness="0.02" FillGradientMode="Vertical" ExposesOutlineColorAsProperty="true" Geometry="RoundedRectangle">
      <Properties>
        <DomainProperty Id="0d9e4c53-2398-4ac1-906d-6a29e8915942" Description="Show or jide the events for this aggregate" Name="EventsVisible" DisplayName="Events Visible" DefaultValue="true" Category="Diagram" IsBrowsable="false">
          <Notes>Are the events of this aggregate shown or collapsed</Notes>
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="882caa4d-32a2-43ba-8858-6c9a6d6def12" Description="Should the projections linked to this aggregate be visible or collapsed" Name="ProjectionsVisible" DisplayName="Projections Visible" DefaultValue="true" Category="Diagram" IsBrowsable="false">
          <Notes>Are the projections linked to this aggregate visible or collapsed</Notes>
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="c3a0925a-bb63-49d0-961b-f8fb8c7659d6" Description="Are the queries linked to this attribute shown or hidden?" Name="QueriesVisible" DisplayName="Queries Visible" DefaultValue="true" Category="Diagram" IsBrowsable="false">
          <Notes>Controls whether or not to show the queries linked to this attribute</Notes>
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="cd776413-5f39-44d1-b6f3-d0a14b9dda32" Description="Description for CQRSAzure.CQRSdsl.Dsl.AggregateGeometryShape.Commands Visible" Name="CommandsVisible" DisplayName="Commands Visible" DefaultValue="true" Category="Diagram" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="bd1aa9dc-3c68-4fd9-8fce-aeaaa0a492c6" Description="Description for CQRSAzure.CQRSdsl.Dsl.AggregateGeometryShape.Outline Color" Name="OutlineColor" DisplayName="Outline Color" Kind="CustomStorage">
          <Type>
            <ExternalTypeMoniker Name="/System.Drawing/Color" />
          </Type>
        </DomainProperty>
      </Properties>
      <ShapeHasDecorators Position="InnerTopCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="NameTextDecorator" DisplayName="Name Text Decorator" DefaultText="Name" FontStyle="Bold" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="Center" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="DescriptionTextDecorator" DisplayName="Description Text Decorator" DefaultText="DescriptionTextDecorator" FontStyle="Italic" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="AggregateIconDecorator" DisplayName="Aggregate Icon Decorator" DefaultIcon="Resources\AggregateShapeToolBitmap.bmp" />
      </ShapeHasDecorators>
    </GeometryShape>
    <CompartmentShape Id="55b19e7f-9b37-438c-a06e-ba276ac6822c" Description="Diagram shape for a query definition" Name="QueryDefinitionShape" DisplayName="Query Definition Shape" Namespace="CQRSAzure.CQRSdsl.Dsl" GeneratesDoubleDerived="true" TooltipType="Variable" FixedTooltipText="Query Definition Shape" FillColor="DeepSkyBlue" InitialHeight="0.5" OutlineThickness="0.02" FillGradientMode="Vertical" Geometry="RoundedRectangle">
      <ShapeHasDecorators Position="OuterTopCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="NameTextDecorator" DisplayName="Name Text Decorator" DefaultText="NameTextDecorator" FontStyle="Bold" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="DescriptionTextDecorator" DisplayName="Description Text Decorator" DefaultText="DescriptionTextDecorator" FontStyle="Italic" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="QueryDefinitionIconDecorator" DisplayName="Query Definition Icon Decorator" DefaultIcon="Resources\QueryShapeToolBitmap.bmp" />
      </ShapeHasDecorators>
      <Compartment TitleFillColor="BlueViolet" Name="InputParametersCompartment" Title="Input Parameters">
        <Notes>The parameters passed in to the query</Notes>
      </Compartment>
      <Compartment TitleFillColor="DodgerBlue" Name="OutputParametersCompartment" Title="Output Properties">
        <Notes>The properties returned from the query</Notes>
      </Compartment>
    </CompartmentShape>
    <CompartmentShape Id="7f2a3f6a-fe87-4d23-812e-f1ed186d34c4" Description="Shape used to denote an event definition on the model diagram" Name="EventDefinitionCompartmentShape" DisplayName="Event Definition" Namespace="CQRSAzure.CQRSdsl.Dsl" GeneratesDoubleDerived="true" TooltipType="Variable" FixedTooltipText="Event Definition" FillColor="Gold" InitialHeight="0.5" OutlineThickness="0.02" FillGradientMode="Vertical" Geometry="RoundedRectangle">
      <ShapeHasDecorators Position="OuterTopCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="NameTextDecorator" DisplayName="Name Text Decorator" DefaultText="NameTextDecorator" FontStyle="Bold" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="DescriptionTextDecorator" DisplayName="Description Text Decorator" DefaultText="DescriptionTextDecorator" FontStyle="Italic" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopRight" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="VersionNumberDecorator" DisplayName="Version Number" DefaultText="1">
          <Notes>The version number of the event</Notes>
        </TextDecorator>
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="EventDefinitionIconDecorator" DisplayName="Event Definition Icon" DefaultIcon="Resources\EventShapeToolBitmap.bmp" />
      </ShapeHasDecorators>
      <Compartment Name="EventProperties" Title="Properties" />
    </CompartmentShape>
    <CompartmentShape Id="f41eec59-8565-444e-9f0a-6f8632721aa3" Description="Diagram element for a projection definition" Name="ProjectionDefinitionCompartmentShape" DisplayName="Projection Definition" HelpKeyword="Projection" Namespace="CQRSAzure.CQRSdsl.Dsl" GeneratesDoubleDerived="true" TooltipType="Variable" FixedTooltipText="Projection Definition " FillColor="LightGreen" InitialHeight="0.5" OutlineThickness="0.01" FillGradientMode="Vertical" Geometry="RoundedRectangle">
      <Notes>Defines how a projection appears on the DSL diagram</Notes>
      <ShapeHasDecorators Position="OuterTopCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="NameTextDecorator" DisplayName="Name" DefaultText="Name" FontStyle="Bold">
          <Notes>The unique name of the projection</Notes>
        </TextDecorator>
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="DescriptionTextDecorator" DisplayName="Description" DefaultText="Description" FontStyle="Italic">
          <Notes>The business description of the projection</Notes>
        </TextDecorator>
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="ProjectionDefinitionIconDecorator" DisplayName="Projection Definition Icon" DefaultIcon="Resources\ProjectionShapeToolBitmap.bmp" />
      </ShapeHasDecorators>
      <Compartment Name="ProjectionProperties" DefaultExpandCollapseState="Collapsed" Title="Properties">
        <Notes>Properties returned by this projection that can be queries against</Notes>
      </Compartment>
      <Compartment Name="PropertyOperations" Title="Event Property Operations">
        <Notes>The operations performed on the projection properties when an event is handled</Notes>
      </Compartment>
    </CompartmentShape>
    <CompartmentShape Id="4595641f-47ea-4a45-8277-6f86115c598f" Description="Description for CQRSAzure.CQRSdsl.Dsl.CommandDefinitionCompartmentShape" Name="CommandDefinitionCompartmentShape" DisplayName="Command Definition Compartment Shape" Namespace="CQRSAzure.CQRSdsl.Dsl" GeneratesDoubleDerived="true" TooltipType="Variable" FixedTooltipText="Command Definition" FillColor="Crimson" InitialHeight="0.5" FillGradientMode="Vertical" Geometry="RoundedRectangle">
      <ShapeHasDecorators Position="OuterTopCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="NameTextDecorator" DisplayName="Name Text Decorator" DefaultText="NameTextDecorator" FontStyle="Bold" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="DescriptionTextDecorator" DisplayName="Description Text Decorator" DefaultText="DescriptionTextDecorator" FontStyle="Italic" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="CommandDefinitionIconDecorator" DisplayName="Command Definition Icon" DefaultIcon="Resources\CommandShapeToolBitmap.bmp" />
      </ShapeHasDecorators>
      <Compartment Name="PropertiesCompartment" Title="Parameters">
        <Notes>Properties to pass to the command</Notes>
      </Compartment>
    </CompartmentShape>
    <GeometryShape Id="9e00f4f3-1d92-4bc1-8bd4-54af0f427fa0" Description="A business grouping of aggregate identities" Name="IdentityGroupGeometryShape" DisplayName="Identity Group Geometry Shape" Namespace="CQRSAzure.CQRSdsl.Dsl" GeneratesDoubleDerived="true" TooltipType="Variable" FixedTooltipText="Identity Group Geometry Shape" FillColor="GreenYellow" InitialHeight="0.5" Geometry="RoundedRectangle">
      <ShapeHasDecorators Position="OuterTopCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="NameTextDecorator" DisplayName="Name Text Decorator" DefaultText="NameTextDecorator">
          <Notes>The name of the identity group</Notes>
        </TextDecorator>
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="Center" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="DescriptionTextDecorator" DisplayName="Description Text Decorator" DefaultText="DescriptionTextDecorator" />
      </ShapeHasDecorators>
    </GeometryShape>
    <CompartmentShape Id="8eb9c78d-a4bc-4475-9618-7bb636e6918c" Description="Designer for the properties of an identity group classifier" Name="ClassifierCompartmentShape" DisplayName="Classifier Compartment Shape" Namespace="CQRSAzure.CQRSdsl.Dsl" TooltipType="Variable" FixedTooltipText="Classifier Compartment Shape" FillColor="Moccasin" InitialHeight="0.5" OutlineThickness="0.01" FillGradientMode="Vertical" Geometry="RoundedRectangle">
      <ShapeHasDecorators Position="OuterTopCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="NameTextDecorator" DisplayName="Name Text Decorator" DefaultText="NameTextDecorator">
          <Notes>The name of the classifier</Notes>
        </TextDecorator>
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="DescriptionTextDecorator" DisplayName="Description Text Decorator" DefaultText="DescriptionTextDecorator">
          <Notes>The description of the classifier for the identity group</Notes>
        </TextDecorator>
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="ClassifierIconDecorator" DisplayName="Classifier Icon Decorator" DefaultIcon="Resources\IdentityGroupToolBitmap.bmp" />
      </ShapeHasDecorators>
      <Compartment TitleFillColor="Orange" Name="EventEvaluationsCompartment" Title="Event Evaluations" />
    </CompartmentShape>
  </Shapes>
  <Connectors>
    <Connector Id="64056196-7601-4f7b-9423-5de8b8c7fab8" Description="Allows one aggregate to be marked as parent of another" Name="AggregateParenthoodConnector" DisplayName="Aggregate Parenthood Connector" Namespace="CQRSAzure.CQRSdsl.Dsl" GeneratesDoubleDerived="true" TooltipType="Variable" FixedTooltipText="Aggregate Parenthood Connector" Color="PaleGoldenrod" DashStyle="Dash" Thickness="0.02" />
    <Connector Id="9ea41467-85da-4d94-8906-a4653f596b38" Description="Event connected to this aggregate" Name="AggregateEventConnector" DisplayName="Aggregate Event" Namespace="CQRSAzure.CQRSdsl.Dsl" GeneratesDoubleDerived="true" TooltipType="Variable" FixedTooltipText="Aggregate Event" Color="Gold" Thickness="0.02" />
    <Connector Id="e04e4806-8b01-4c01-ae60-f4e9de82d449" Description="Projection for this aggregate root" Name="AggregateProjectionConnector" DisplayName="Aggregate Projection" Namespace="CQRSAzure.CQRSdsl.Dsl" GeneratesDoubleDerived="true" TooltipType="Variable" FixedTooltipText="Aggregate Projection Connector" Color="LightGreen" TargetEndStyle="FilledArrow" Thickness="0.02" />
    <Connector Id="e72dd32c-bc67-4724-87e7-51b959740a41" Description="Link between an aggregate identifier and the query definition(s) designed to run against it" Name="AggregateQueryDefinitionConnector" DisplayName="Aggregate Query Definition Connector" Namespace="CQRSAzure.CQRSdsl.Dsl" GeneratesDoubleDerived="true" TooltipType="Variable" FixedTooltipText="Aggregate Query Definition Connector" Color="DeepSkyBlue" TargetEndStyle="HollowArrow" Thickness="0.01" />
    <Connector Id="febd39ed-3ae0-41b4-afc8-5ab0585ddb4f" Description="Description for CQRSAzure.CQRSdsl.Dsl.AggregateCommandDefinitionConnector" Name="AggregateCommandDefinitionConnector" DisplayName="Aggregate Command Definition Connector" Namespace="CQRSAzure.CQRSdsl.Dsl" GeneratesDoubleDerived="true" TooltipType="Variable" FixedTooltipText="Aggregate Command Definition Connector" Color="Crimson" SourceEndStyle="FilledArrow" Thickness="0.01" />
    <Connector Id="aa757e86-eb07-449c-8a68-9fcf93c9aaa7" Description="Events handled by the projection" Name="ProjectionEventConnector" DisplayName="Projection Event Connector" Namespace="CQRSAzure.CQRSdsl.Dsl" GeneratesDoubleDerived="true" TooltipType="Variable" FixedTooltipText="Projection Event Connector" DashStyle="Dot" Thickness="0.01" />
    <Connector Id="a9d56ec1-c552-40e5-9608-eee4cfd9f0e6" Description="Connection between an aggregate identifier definition and the identity groups that may contain it" Name="AggregateIdentityGroupConnector" DisplayName="Aggregate Identity Group Connector" Namespace="CQRSAzure.CQRSdsl.Dsl" FixedTooltipText="Aggregate Identity Group Connector" Color="LightGreen" Thickness="0.01" />
    <Connector Id="fc29b546-efd7-4f5b-a7be-121a06430c74" Description="Link between an identity group and the classifier that generates its membership" Name="IdentityGroupClassifierConnector" DisplayName="Identity Group Classifier Connector" Namespace="CQRSAzure.CQRSdsl.Dsl" FixedTooltipText="Identity Group Classifier" DashStyle="Dash" Thickness="0.01" />
    <Connector Id="521969b1-9129-4bf3-91ba-e1508111fa56" Description="Connection between a classifier and an event it handles" Name="ClassifierEventConnector" DisplayName="Classifier Event Connector" Namespace="CQRSAzure.CQRSdsl.Dsl" FixedTooltipText="Classifier Event Connector" Color="BlanchedAlmond" DashStyle="Dot" Thickness="0.01" RoutingStyle="Straight" />
  </Connectors>
  <XmlSerializationBehavior Name="CQRSdslSerializationBehavior" Namespace="CQRSAzure.CQRSdsl.Dsl">
    <ClassData>
      <XmlClassData TypeName="CQRSModel" MonikerAttributeName="" SerializeId="true" MonikerElementName="cQRSModelMoniker" ElementName="cQRSModel" MonikerTypeName="CQRSModelMoniker">
        <DomainClassMoniker Name="CQRSModel" />
        <ElementData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="aggregateIdentifiers">
            <DomainRelationshipMoniker Name="CQRSModelHasAggregateIdentifiers" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="CQRSModel/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="notes">
            <DomainPropertyMoniker Name="CQRSModel/Notes" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="modelSet">
            <DomainRelationshipMoniker Name="CQRSModelHasModelSet" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="description">
            <DomainPropertyMoniker Name="CQRSModel/Description" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="CQRSdslDiagram" MonikerAttributeName="" SerializeId="true" MonikerElementName="cQRSdslDiagramMoniker" ElementName="cQRSdslDiagram" MonikerTypeName="CQRSdslDiagramMoniker">
        <DiagramMoniker Name="CQRSdslDiagram" />
        <ElementData>
          <XmlPropertyData XmlName="outputCodeLanguage">
            <DomainPropertyMoniker Name="CQRSdslDiagram/OutputCodeLanguage" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="copyrightNotice">
            <DomainPropertyMoniker Name="CQRSdslDiagram/CopyrightNotice" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="companyName">
            <DomainPropertyMoniker Name="CQRSdslDiagram/CompanyName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="subfolderPerModel">
            <DomainPropertyMoniker Name="CQRSdslDiagram/SubfolderPerModel" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="subfolderPerAggregate">
            <DomainPropertyMoniker Name="CQRSdslDiagram/SubfolderPerAggregate" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="AggregateIdentifier" MonikerAttributeName="name" SerializeId="true" MonikerElementName="aggregateIdentifierMoniker" ElementName="aggregateIdentifier" MonikerTypeName="AggregateIdentifierMoniker">
        <DomainClassMoniker Name="AggregateIdentifier" />
        <ElementData>
          <XmlPropertyData XmlName="name" IsMonikerKey="true">
            <DomainPropertyMoniker Name="AggregateIdentifier/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="description">
            <DomainPropertyMoniker Name="AggregateIdentifier/Description" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="keyName">
            <DomainPropertyMoniker Name="AggregateIdentifier/KeyName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="keyDataType">
            <DomainPropertyMoniker Name="AggregateIdentifier/KeyDataType" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="eventDefinitions">
            <DomainRelationshipMoniker Name="AggregateIdentifierHasEventDefinitions" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="projectionDefinitions">
            <DomainRelationshipMoniker Name="AggregateIdentifierHasProjectionDefinitions" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="targetAggregateIdentifiers">
            <DomainRelationshipMoniker Name="AggregateIdentifierIsChildOfTargetAggregateIdentifiers" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="commandDefinitions">
            <DomainRelationshipMoniker Name="AggregateIdentifierHasCommandDefinitions" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="queryDefinitions">
            <DomainRelationshipMoniker Name="AggregateIdentifierHasQueryDefinitions" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="notes">
            <DomainPropertyMoniker Name="AggregateIdentifier/Notes" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="identityGrouped">
            <DomainRelationshipMoniker Name="AggregateIdentifierHasIdentityGrouped" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="classifiers">
            <DomainRelationshipMoniker Name="AggregateIdentifierHasClassifiers" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="category">
            <DomainPropertyMoniker Name="AggregateIdentifier/Category" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="AggregateGeometryShape" MonikerAttributeName="" SerializeId="true" MonikerElementName="aggregateGeometryShapeMoniker" ElementName="aggregateGeometryShape" MonikerTypeName="AggregateGeometryShapeMoniker">
        <GeometryShapeMoniker Name="AggregateGeometryShape" />
        <ElementData>
          <XmlPropertyData XmlName="eventsVisible">
            <DomainPropertyMoniker Name="AggregateGeometryShape/EventsVisible" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="projectionsVisible">
            <DomainPropertyMoniker Name="AggregateGeometryShape/ProjectionsVisible" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="queriesVisible">
            <DomainPropertyMoniker Name="AggregateGeometryShape/QueriesVisible" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="commandsVisible">
            <DomainPropertyMoniker Name="AggregateGeometryShape/CommandsVisible" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="outlineColor">
            <DomainPropertyMoniker Name="AggregateGeometryShape/OutlineColor" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="CQRSModelHasAggregateIdentifiers" MonikerAttributeName="" SerializeId="true" MonikerElementName="cQRSModelHasAggregateIdentifiersMoniker" ElementName="cQRSModelHasAggregateIdentifiers" MonikerTypeName="CQRSModelHasAggregateIdentifiersMoniker">
        <DomainRelationshipMoniker Name="CQRSModelHasAggregateIdentifiers" />
      </XmlClassData>
      <XmlClassData TypeName="EventDefinition" MonikerAttributeName="name" SerializeId="true" MonikerElementName="eventDefinitionMoniker" ElementName="eventDefinition" MonikerTypeName="EventDefinitionMoniker">
        <DomainClassMoniker Name="EventDefinition" />
        <ElementData>
          <XmlPropertyData XmlName="name" IsMonikerKey="true">
            <DomainPropertyMoniker Name="EventDefinition/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="description">
            <DomainPropertyMoniker Name="EventDefinition/Description" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="notes">
            <DomainPropertyMoniker Name="EventDefinition/Notes" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="version">
            <DomainPropertyMoniker Name="EventDefinition/Version" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="eventProperties">
            <DomainRelationshipMoniker Name="EventDefinitionHasEventProperties" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="category">
            <DomainPropertyMoniker Name="EventDefinition/Category" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="AggregateIdentifierHasEventDefinitions" MonikerAttributeName="" SerializeId="true" MonikerElementName="aggregateIdentifierHasEventDefinitionsMoniker" ElementName="aggregateIdentifierHasEventDefinitions" MonikerTypeName="AggregateIdentifierHasEventDefinitionsMoniker">
        <DomainRelationshipMoniker Name="AggregateIdentifierHasEventDefinitions" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="AggregateIdentifierHasEventDefinitions/Name" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ProjectionDefinition" MonikerAttributeName="name" SerializeId="true" MonikerElementName="projectionDefinitionMoniker" ElementName="projectionDefinition" MonikerTypeName="ProjectionDefinitionMoniker">
        <DomainClassMoniker Name="ProjectionDefinition" />
        <ElementData>
          <XmlPropertyData XmlName="name" IsMonikerKey="true">
            <DomainPropertyMoniker Name="ProjectionDefinition/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="description">
            <DomainPropertyMoniker Name="ProjectionDefinition/Description" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="notes">
            <DomainPropertyMoniker Name="ProjectionDefinition/Notes" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="projectionProperties">
            <DomainRelationshipMoniker Name="ProjectionDefinitionHasProjectionProperties" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="eventDefinitions">
            <DomainRelationshipMoniker Name="ProjectionDefinitionHandlesEventDefinitions" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="projectionEventPropertyOperations">
            <DomainRelationshipMoniker Name="ProjectionDefinitionHasEventPropertyOperations" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="category">
            <DomainPropertyMoniker Name="ProjectionDefinition/Category" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="AggregateIdentifierHasProjectionDefinitions" MonikerAttributeName="" SerializeId="true" MonikerElementName="aggregateIdentifierHasProjectionDefinitionsMoniker" ElementName="aggregateIdentifierHasProjectionDefinitions" MonikerTypeName="AggregateIdentifierHasProjectionDefinitionsMoniker">
        <DomainRelationshipMoniker Name="AggregateIdentifierHasProjectionDefinitions" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="AggregateIdentifierHasProjectionDefinitions/Name" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="AggregateIdentifierIsChildOfTargetAggregateIdentifiers" MonikerAttributeName="" SerializeId="true" MonikerElementName="aggregateIdentifierIsChildOfTargetAggregateIdentifiersMoniker" ElementName="aggregateIdentifierIsChildOfTargetAggregateIdentifiers" MonikerTypeName="AggregateIdentifierIsChildOfTargetAggregateIdentifiersMoniker">
        <DomainRelationshipMoniker Name="AggregateIdentifierIsChildOfTargetAggregateIdentifiers" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="AggregateIdentifierIsChildOfTargetAggregateIdentifiers/Name" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="AggregateParenthoodConnector" MonikerAttributeName="" SerializeId="true" MonikerElementName="aggregateParenthoodConnectorMoniker" ElementName="aggregateParenthoodConnector" MonikerTypeName="AggregateParenthoodConnectorMoniker">
        <ConnectorMoniker Name="AggregateParenthoodConnector" />
      </XmlClassData>
      <XmlClassData TypeName="AggregateEventConnector" MonikerAttributeName="" SerializeId="true" MonikerElementName="aggregateEventConnectorMoniker" ElementName="aggregateEventConnector" MonikerTypeName="AggregateEventConnectorMoniker">
        <ConnectorMoniker Name="AggregateEventConnector" />
      </XmlClassData>
      <XmlClassData TypeName="AggregateProjectionConnector" MonikerAttributeName="" SerializeId="true" MonikerElementName="aggregateProjectionConnectorMoniker" ElementName="aggregateProjectionConnector" MonikerTypeName="AggregateProjectionConnectorMoniker">
        <ConnectorMoniker Name="AggregateProjectionConnector" />
      </XmlClassData>
      <XmlClassData TypeName="CommandDefinition" MonikerAttributeName="name" SerializeId="true" MonikerElementName="commandDefinitionMoniker" ElementName="commandDefinition" MonikerTypeName="CommandDefinitionMoniker">
        <DomainClassMoniker Name="CommandDefinition" />
        <ElementData>
          <XmlPropertyData XmlName="name" IsMonikerKey="true">
            <DomainPropertyMoniker Name="CommandDefinition/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="description">
            <DomainPropertyMoniker Name="CommandDefinition/Description" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="commandParameters">
            <DomainRelationshipMoniker Name="CommandDefinitionHasParameters" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="notes">
            <DomainPropertyMoniker Name="CommandDefinition/Notes" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="identityGroup">
            <DomainRelationshipMoniker Name="CommandDefinitionReferencesIdentityGroup" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="category">
            <DomainPropertyMoniker Name="CommandDefinition/Category" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="AggregateIdentifierHasCommandDefinitions" MonikerAttributeName="" SerializeId="true" MonikerElementName="aggregateIdentifierHasCommandDefinitionsMoniker" ElementName="aggregateIdentifierHasCommandDefinitions" MonikerTypeName="AggregateIdentifierHasCommandDefinitionsMoniker">
        <DomainRelationshipMoniker Name="AggregateIdentifierHasCommandDefinitions" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="AggregateIdentifierHasCommandDefinitions/Name" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EventProperty" MonikerAttributeName="name" SerializeId="true" MonikerElementName="eventPropertyMoniker" ElementName="eventProperty" MonikerTypeName="EventPropertyMoniker">
        <DomainClassMoniker Name="EventProperty" />
        <ElementData>
          <XmlPropertyData XmlName="name" IsMonikerKey="true">
            <DomainPropertyMoniker Name="EventProperty/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="description">
            <DomainPropertyMoniker Name="EventProperty/Description" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="dataType">
            <DomainPropertyMoniker Name="EventProperty/DataType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="notes">
            <DomainPropertyMoniker Name="EventProperty/Notes" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EventDefinitionHasEventProperties" MonikerAttributeName="" SerializeId="true" MonikerElementName="eventDefinitionHasEventPropertiesMoniker" ElementName="eventDefinitionHasEventProperties" MonikerTypeName="EventDefinitionHasEventPropertiesMoniker">
        <DomainRelationshipMoniker Name="EventDefinitionHasEventProperties" />
        <ElementData>
          <XmlPropertyData XmlName="createdVersion">
            <DomainPropertyMoniker Name="EventDefinitionHasEventProperties/CreatedVersion" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="depreciatedVersion">
            <DomainPropertyMoniker Name="EventDefinitionHasEventProperties/DepreciatedVersion" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="EventDefinitionHasEventProperties/Name" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ProjectionProperty" MonikerAttributeName="name" SerializeId="true" MonikerElementName="projectionPropertyMoniker" ElementName="projectionProperty" MonikerTypeName="ProjectionPropertyMoniker">
        <DomainClassMoniker Name="ProjectionProperty" />
        <ElementData>
          <XmlPropertyData XmlName="name" IsMonikerKey="true">
            <DomainPropertyMoniker Name="ProjectionProperty/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="description">
            <DomainPropertyMoniker Name="ProjectionProperty/Description" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="dataType">
            <DomainPropertyMoniker Name="ProjectionProperty/DataType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="notes">
            <DomainPropertyMoniker Name="ProjectionProperty/Notes" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ProjectionDefinitionHasProjectionProperties" MonikerAttributeName="" SerializeId="true" MonikerElementName="projectionDefinitionHasProjectionPropertiesMoniker" ElementName="projectionDefinitionHasProjectionProperties" MonikerTypeName="ProjectionDefinitionHasProjectionPropertiesMoniker">
        <DomainRelationshipMoniker Name="ProjectionDefinitionHasProjectionProperties" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="ProjectionDefinitionHasProjectionProperties/Name" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="CommandParameter" MonikerAttributeName="name" SerializeId="true" MonikerElementName="commandParameterMoniker" ElementName="commandParameter" MonikerTypeName="CommandParameterMoniker">
        <DomainClassMoniker Name="CommandParameter" />
        <ElementData>
          <XmlPropertyData XmlName="name" IsMonikerKey="true">
            <DomainPropertyMoniker Name="CommandParameter/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="description">
            <DomainPropertyMoniker Name="CommandParameter/Description" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="parameterType">
            <DomainPropertyMoniker Name="CommandParameter/ParameterType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="notes">
            <DomainPropertyMoniker Name="CommandParameter/Notes" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isAggregateKey">
            <DomainPropertyMoniker Name="CommandParameter/IsAggregateKey" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="CommandDefinitionHasParameters" MonikerAttributeName="" SerializeId="true" MonikerElementName="commandDefinitionHasParametersMoniker" ElementName="commandDefinitionHasParameters" MonikerTypeName="CommandDefinitionHasParametersMoniker">
        <DomainRelationshipMoniker Name="CommandDefinitionHasParameters" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="CommandDefinitionHasParameters/Name" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="QueryDefinition" MonikerAttributeName="name" SerializeId="true" MonikerElementName="queryDefinitionMoniker" ElementName="queryDefinition" MonikerTypeName="QueryDefinitionMoniker">
        <DomainClassMoniker Name="QueryDefinition" />
        <ElementData>
          <XmlPropertyData XmlName="name" IsMonikerKey="true">
            <DomainPropertyMoniker Name="QueryDefinition/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="description">
            <DomainPropertyMoniker Name="QueryDefinition/Description" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="queryInputParameters">
            <DomainRelationshipMoniker Name="QueryDefinitionHasQueryInputParameters" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="queryReturnParameters">
            <DomainRelationshipMoniker Name="QueryDefinitionHasQueryReturnParameters" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="multiRowResults">
            <DomainPropertyMoniker Name="QueryDefinition/MultiRowResults" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="notes">
            <DomainPropertyMoniker Name="QueryDefinition/Notes" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="identityGroup">
            <DomainRelationshipMoniker Name="QueryDefinitionReferencesIdentityGroup" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="projectionDefinition">
            <DomainRelationshipMoniker Name="QueryDefinitionReferencesProjectionDefinition" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="category">
            <DomainPropertyMoniker Name="QueryDefinition/Category" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="AggregateIdentifierHasQueryDefinitions" MonikerAttributeName="" SerializeId="true" MonikerElementName="aggregateIdentifierHasQueryDefinitionsMoniker" ElementName="aggregateIdentifierHasQueryDefinitions" MonikerTypeName="AggregateIdentifierHasQueryDefinitionsMoniker">
        <DomainRelationshipMoniker Name="AggregateIdentifierHasQueryDefinitions" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="AggregateIdentifierHasQueryDefinitions/Name" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="QueryInputParameter" MonikerAttributeName="name" SerializeId="true" MonikerElementName="queryInputParameterMoniker" ElementName="queryInputParameter" MonikerTypeName="QueryInputParameterMoniker">
        <DomainClassMoniker Name="QueryInputParameter" />
        <ElementData>
          <XmlPropertyData XmlName="name" IsMonikerKey="true">
            <DomainPropertyMoniker Name="QueryInputParameter/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="description">
            <DomainPropertyMoniker Name="QueryInputParameter/Description" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="dataType">
            <DomainPropertyMoniker Name="QueryInputParameter/DataType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isAggregateKey">
            <DomainPropertyMoniker Name="QueryInputParameter/IsAggregateKey" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="notes">
            <DomainPropertyMoniker Name="QueryInputParameter/Notes" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isEffectiveDate">
            <DomainPropertyMoniker Name="QueryInputParameter/IsEffectiveDate" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="QueryDefinitionHasQueryInputParameters" MonikerAttributeName="" SerializeId="true" MonikerElementName="queryDefinitionHasQueryInputParametersMoniker" ElementName="queryDefinitionHasQueryInputParameters" MonikerTypeName="QueryDefinitionHasQueryInputParametersMoniker">
        <DomainRelationshipMoniker Name="QueryDefinitionHasQueryInputParameters" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="QueryDefinitionHasQueryInputParameters/Name" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="QueryDefinitionShape" MonikerAttributeName="" SerializeId="true" MonikerElementName="queryDefinitionShapeMoniker" ElementName="queryDefinitionShape" MonikerTypeName="QueryDefinitionShapeMoniker">
        <CompartmentShapeMoniker Name="QueryDefinitionShape" />
      </XmlClassData>
      <XmlClassData TypeName="AggregateQueryDefinitionConnector" MonikerAttributeName="" SerializeId="true" MonikerElementName="aggregateQueryDefinitionConnectorMoniker" ElementName="aggregateQueryDefinitionConnector" MonikerTypeName="AggregateQueryDefinitionConnectorMoniker">
        <ConnectorMoniker Name="AggregateQueryDefinitionConnector" />
      </XmlClassData>
      <XmlClassData TypeName="AggregateCommandDefinitionConnector" MonikerAttributeName="" SerializeId="true" MonikerElementName="aggregateCommandDefinitionConnectorMoniker" ElementName="aggregateCommandDefinitionConnector" MonikerTypeName="AggregateCommandDefinitionConnectorMoniker">
        <ConnectorMoniker Name="AggregateCommandDefinitionConnector" />
      </XmlClassData>
      <XmlClassData TypeName="EventDefinitionCompartmentShape" MonikerAttributeName="" SerializeId="true" MonikerElementName="eventDefinitionCompartmentShapeMoniker" ElementName="eventDefinitionCompartmentShape" MonikerTypeName="EventDefinitionCompartmentShapeMoniker">
        <CompartmentShapeMoniker Name="EventDefinitionCompartmentShape" />
      </XmlClassData>
      <XmlClassData TypeName="ProjectionDefinitionCompartmentShape" MonikerAttributeName="" SerializeId="true" MonikerElementName="projectionDefinitionCompartmentShapeMoniker" ElementName="projectionDefinitionCompartmentShape" MonikerTypeName="ProjectionDefinitionCompartmentShapeMoniker">
        <CompartmentShapeMoniker Name="ProjectionDefinitionCompartmentShape" />
      </XmlClassData>
      <XmlClassData TypeName="CommandDefinitionCompartmentShape" MonikerAttributeName="" SerializeId="true" MonikerElementName="commandDefinitionCompartmentShapeMoniker" ElementName="commandDefinitionCompartmentShape" MonikerTypeName="CommandDefinitionCompartmentShapeMoniker">
        <CompartmentShapeMoniker Name="CommandDefinitionCompartmentShape" />
      </XmlClassData>
      <XmlClassData TypeName="ProjectionDefinitionHandlesEventDefinitions" MonikerAttributeName="" SerializeId="true" MonikerElementName="projectionDefinitionHandlesEventDefinitionsMoniker" ElementName="projectionDefinitionHandlesEventDefinitions" MonikerTypeName="ProjectionDefinitionHandlesEventDefinitionsMoniker">
        <DomainRelationshipMoniker Name="ProjectionDefinitionHandlesEventDefinitions" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="ProjectionDefinitionHandlesEventDefinitions/Name" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ProjectionEventConnector" MonikerAttributeName="" SerializeId="true" MonikerElementName="projectionEventConnectorMoniker" ElementName="projectionEventConnector" MonikerTypeName="ProjectionEventConnectorMoniker">
        <ConnectorMoniker Name="ProjectionEventConnector" />
      </XmlClassData>
      <XmlClassData TypeName="ProjectionEventPropertyOperation" MonikerAttributeName="" SerializeId="true" MonikerElementName="projectionEventPropertyOperationMoniker" ElementName="projectionEventPropertyOperation" MonikerTypeName="ProjectionEventPropertyOperationMoniker">
        <DomainClassMoniker Name="ProjectionEventPropertyOperation" />
        <ElementData>
          <XmlPropertyData XmlName="eventName">
            <DomainPropertyMoniker Name="ProjectionEventPropertyOperation/EventName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="sourceEventPropertyName">
            <DomainPropertyMoniker Name="ProjectionEventPropertyOperation/SourceEventPropertyName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="targetPropertyName">
            <DomainPropertyMoniker Name="ProjectionEventPropertyOperation/TargetPropertyName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="propertyOperationToPerform">
            <DomainPropertyMoniker Name="ProjectionEventPropertyOperation/PropertyOperationToPerform" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="description">
            <DomainPropertyMoniker Name="ProjectionEventPropertyOperation/Description" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="notes">
            <DomainPropertyMoniker Name="ProjectionEventPropertyOperation/Notes" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ProjectionDefinitionHasEventPropertyOperations" MonikerAttributeName="" SerializeId="true" MonikerElementName="projectionDefinitionHasEventPropertyOperationsMoniker" ElementName="projectionDefinitionHasEventPropertyOperations" MonikerTypeName="ProjectionDefinitionHasEventPropertyOperationsMoniker">
        <DomainRelationshipMoniker Name="ProjectionDefinitionHasEventPropertyOperations" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="ProjectionDefinitionHasEventPropertyOperations/Name" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="QueryReturnParameter" MonikerAttributeName="" SerializeId="true" MonikerElementName="queryReturnParameterMoniker" ElementName="queryReturnParameter" MonikerTypeName="QueryReturnParameterMoniker">
        <DomainClassMoniker Name="QueryReturnParameter" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="QueryReturnParameter/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="description">
            <DomainPropertyMoniker Name="QueryReturnParameter/Description" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="dataType">
            <DomainPropertyMoniker Name="QueryReturnParameter/DataType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="notes">
            <DomainPropertyMoniker Name="QueryReturnParameter/Notes" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="QueryDefinitionHasQueryReturnParameters" MonikerAttributeName="" SerializeId="true" MonikerElementName="queryDefinitionHasQueryReturnParametersMoniker" ElementName="queryDefinitionHasQueryReturnParameters" MonikerTypeName="QueryDefinitionHasQueryReturnParametersMoniker">
        <DomainRelationshipMoniker Name="QueryDefinitionHasQueryReturnParameters" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="QueryDefinitionHasQueryReturnParameters/Name" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ModelSetting" MonikerAttributeName="" SerializeId="true" MonikerElementName="modelSettingMoniker" ElementName="modelSetting" MonikerTypeName="ModelSettingMoniker">
        <DomainClassMoniker Name="ModelSetting" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="ModelSetting/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="value">
            <DomainPropertyMoniker Name="ModelSetting/Value" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="CQRSModelHasModelSet" MonikerAttributeName="" SerializeId="true" MonikerElementName="cQRSModelHasModelSetMoniker" ElementName="cQRSModelHasModelSet" MonikerTypeName="CQRSModelHasModelSetMoniker">
        <DomainRelationshipMoniker Name="CQRSModelHasModelSet" />
      </XmlClassData>
      <XmlClassData TypeName="IdentityGroup" MonikerAttributeName="" SerializeId="true" MonikerElementName="identityGroupMoniker" ElementName="identityGroup" MonikerTypeName="IdentityGroupMoniker">
        <DomainClassMoniker Name="IdentityGroup" />
        <ElementData>
          <XmlPropertyData XmlName="isInstance">
            <DomainPropertyMoniker Name="IdentityGroup/IsInstance" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isGlobal">
            <DomainPropertyMoniker Name="IdentityGroup/IsGlobal" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="IdentityGroup/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="description">
            <DomainPropertyMoniker Name="IdentityGroup/Description" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="notes">
            <DomainPropertyMoniker Name="IdentityGroup/Notes" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="classifier">
            <DomainRelationshipMoniker Name="IdentityGroupReferencesClassifier" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="category">
            <DomainPropertyMoniker Name="IdentityGroup/Category" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="QueryDefinitionReferencesIdentityGroup" MonikerAttributeName="" SerializeId="true" MonikerElementName="queryDefinitionReferencesIdentityGroupMoniker" ElementName="queryDefinitionReferencesIdentityGroup" MonikerTypeName="QueryDefinitionReferencesIdentityGroupMoniker">
        <DomainRelationshipMoniker Name="QueryDefinitionReferencesIdentityGroup" />
      </XmlClassData>
      <XmlClassData TypeName="IdentityGroupGeometryShape" MonikerAttributeName="" SerializeId="true" MonikerElementName="identityGroupGeometryShapeMoniker" ElementName="identityGroupGeometryShape" MonikerTypeName="IdentityGroupGeometryShapeMoniker">
        <GeometryShapeMoniker Name="IdentityGroupGeometryShape" />
      </XmlClassData>
      <XmlClassData TypeName="AggregateIdentifierHasIdentityGrouped" MonikerAttributeName="" SerializeId="true" MonikerElementName="aggregateIdentifierHasIdentityGroupedMoniker" ElementName="aggregateIdentifierHasIdentityGrouped" MonikerTypeName="AggregateIdentifierHasIdentityGroupedMoniker">
        <DomainRelationshipMoniker Name="AggregateIdentifierHasIdentityGrouped" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="AggregateIdentifierHasIdentityGrouped/Name" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="AggregateIdentityGroupConnector" MonikerAttributeName="" SerializeId="true" MonikerElementName="aggregateIdentityGroupConnectorMoniker" ElementName="aggregateIdentityGroupConnector" MonikerTypeName="AggregateIdentityGroupConnectorMoniker">
        <ConnectorMoniker Name="AggregateIdentityGroupConnector" />
      </XmlClassData>
      <XmlClassData TypeName="Classifier" MonikerAttributeName="" SerializeId="true" MonikerElementName="classifierMoniker" ElementName="classifier" MonikerTypeName="ClassifierMoniker">
        <DomainClassMoniker Name="Classifier" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="Classifier/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="description">
            <DomainPropertyMoniker Name="Classifier/Description" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="notes">
            <DomainPropertyMoniker Name="Classifier/Notes" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="eventDefinitions">
            <DomainRelationshipMoniker Name="ClassifierHandlesEvents" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="classifierEventEvaluations">
            <DomainRelationshipMoniker Name="ClassifierHasEventEvaluations" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="category">
            <DomainPropertyMoniker Name="Classifier/Category" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="AggregateIdentifierHasClassifiers" MonikerAttributeName="" SerializeId="true" MonikerElementName="aggregateIdentifierHasClassifiersMoniker" ElementName="aggregateIdentifierHasClassifiers" MonikerTypeName="AggregateIdentifierHasClassifiersMoniker">
        <DomainRelationshipMoniker Name="AggregateIdentifierHasClassifiers" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="AggregateIdentifierHasClassifiers/Name" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="IdentityGroupReferencesClassifier" MonikerAttributeName="" SerializeId="true" MonikerElementName="identityGroupReferencesClassifierMoniker" ElementName="identityGroupReferencesClassifier" MonikerTypeName="IdentityGroupReferencesClassifierMoniker">
        <DomainRelationshipMoniker Name="IdentityGroupReferencesClassifier" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="IdentityGroupReferencesClassifier/Name" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ClassifierCompartmentShape" MonikerAttributeName="" SerializeId="true" MonikerElementName="classifierCompartmentShapeMoniker" ElementName="classifierCompartmentShape" MonikerTypeName="ClassifierCompartmentShapeMoniker">
        <CompartmentShapeMoniker Name="ClassifierCompartmentShape" />
      </XmlClassData>
      <XmlClassData TypeName="IdentityGroupClassifierConnector" MonikerAttributeName="" SerializeId="true" MonikerElementName="identityGroupClassifierConnectorMoniker" ElementName="identityGroupClassifierConnector" MonikerTypeName="IdentityGroupClassifierConnectorMoniker">
        <ConnectorMoniker Name="IdentityGroupClassifierConnector" />
      </XmlClassData>
      <XmlClassData TypeName="ClassifierHandlesEvents" MonikerAttributeName="" SerializeId="true" MonikerElementName="classifierHandlesEventsMoniker" ElementName="classifierHandlesEvents" MonikerTypeName="ClassifierHandlesEventsMoniker">
        <DomainRelationshipMoniker Name="ClassifierHandlesEvents" />
      </XmlClassData>
      <XmlClassData TypeName="ClassifierEventEvaluation" MonikerAttributeName="" SerializeId="true" MonikerElementName="classifierEventEvaluationMoniker" ElementName="classifierEventEvaluation" MonikerTypeName="ClassifierEventEvaluationMoniker">
        <DomainClassMoniker Name="ClassifierEventEvaluation" />
        <ElementData>
          <XmlPropertyData XmlName="eventName">
            <DomainPropertyMoniker Name="ClassifierEventEvaluation/EventName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="sourceEventPropertyName">
            <DomainPropertyMoniker Name="ClassifierEventEvaluation/SourceEventPropertyName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="notes">
            <DomainPropertyMoniker Name="ClassifierEventEvaluation/Notes" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="description">
            <DomainPropertyMoniker Name="ClassifierEventEvaluation/Description" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="propertyEvaluationToPerform">
            <DomainPropertyMoniker Name="ClassifierEventEvaluation/PropertyEvaluationToPerform" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="onTrue">
            <DomainPropertyMoniker Name="ClassifierEventEvaluation/OnTrue" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="onFalse">
            <DomainPropertyMoniker Name="ClassifierEventEvaluation/OnFalse" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="target">
            <DomainPropertyMoniker Name="ClassifierEventEvaluation/Target" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="targetType">
            <DomainPropertyMoniker Name="ClassifierEventEvaluation/TargetType" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ClassifierHasEventEvaluations" MonikerAttributeName="" SerializeId="true" MonikerElementName="classifierHasEventEvaluationsMoniker" ElementName="classifierHasEventEvaluations" MonikerTypeName="ClassifierHasEventEvaluationsMoniker">
        <DomainRelationshipMoniker Name="ClassifierHasEventEvaluations" />
      </XmlClassData>
      <XmlClassData TypeName="ClassifierEventConnector" MonikerAttributeName="" SerializeId="true" MonikerElementName="classifierEventConnectorMoniker" ElementName="classifierEventConnector" MonikerTypeName="ClassifierEventConnectorMoniker">
        <ConnectorMoniker Name="ClassifierEventConnector" />
      </XmlClassData>
      <XmlClassData TypeName="CommandDefinitionReferencesIdentityGroup" MonikerAttributeName="" SerializeId="true" MonikerElementName="commandDefinitionReferencesIdentityGroupMoniker" ElementName="commandDefinitionReferencesIdentityGroup" MonikerTypeName="CommandDefinitionReferencesIdentityGroupMoniker">
        <DomainRelationshipMoniker Name="CommandDefinitionReferencesIdentityGroup" />
      </XmlClassData>
      <XmlClassData TypeName="QueryDefinitionReferencesProjectionDefinition" MonikerAttributeName="" SerializeId="true" MonikerElementName="queryDefinitionReferencesProjectionDefinitionMoniker" ElementName="queryDefinitionReferencesProjectionDefinition" MonikerTypeName="QueryDefinitionReferencesProjectionDefinitionMoniker">
        <DomainRelationshipMoniker Name="QueryDefinitionReferencesProjectionDefinition" />
      </XmlClassData>
    </ClassData>
  </XmlSerializationBehavior>
  <ExplorerBehavior Name="CQRSdslExplorer">
    <Notes>This designer allows the creation of CQRS / ES architectures for a domain</Notes>
    <CustomNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\AggregateShapeToolBitmap.bmp" ShowsDomainClass="true">
        <Notes>Identifies a top level aggregate in the domain</Notes>
        <Class>
          <DomainClassMoniker Name="AggregateIdentifier" />
        </Class>
        <PropertyDisplayed>
          <PropertyPath>
            <Notes>The name of the aggregate identifier</Notes>
            <DomainPropertyMoniker Name="AggregateIdentifier/Name" />
            <DomainPath />
          </PropertyPath>
        </PropertyDisplayed>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\EventShapeToolBitmap.bmp" ShowsDomainClass="true">
        <Notes>A recorded event for the aggregate</Notes>
        <Class>
          <DomainClassMoniker Name="EventDefinition" />
        </Class>
        <PropertyDisplayed>
          <PropertyPath>
            <Notes>The event name</Notes>
            <DomainPropertyMoniker Name="EventDefinition/Name" />
            <DomainPath />
          </PropertyPath>
        </PropertyDisplayed>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\CommandShapeToolBitmap.bmp" ShowsDomainClass="true">
        <Notes>A command that can be triggered against an aggregate</Notes>
        <Class>
          <DomainClassMoniker Name="CommandDefinition" />
        </Class>
        <PropertyDisplayed>
          <PropertyPath>
            <Notes>The command name</Notes>
            <DomainPropertyMoniker Name="CommandDefinition/Name" />
            <DomainPath />
          </PropertyPath>
        </PropertyDisplayed>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\ProjectionShapeToolBitmap.bmp" ShowsDomainClass="true">
        <Notes>A projection to run over the events for this aggregate</Notes>
        <Class>
          <DomainClassMoniker Name="ProjectionDefinition" />
        </Class>
        <PropertyDisplayed>
          <PropertyPath>
            <Notes>The name of this projection</Notes>
            <DomainPropertyMoniker Name="ProjectionDefinition/Name" />
            <DomainPath />
          </PropertyPath>
        </PropertyDisplayed>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\QueryShapeToolBitmap.bmp" ShowsDomainClass="true">
        <Notes>A query to run for the given aggregate type</Notes>
        <Class>
          <DomainClassMoniker Name="QueryDefinition" />
        </Class>
        <PropertyDisplayed>
          <PropertyPath>
            <Notes>The name of this query definition</Notes>
            <DomainPropertyMoniker Name="QueryDefinition/Name" />
            <DomainPath />
          </PropertyPath>
        </PropertyDisplayed>
      </ExplorerNodeSettings>
    </CustomNodeSettings>
  </ExplorerBehavior>
  <ConnectionBuilders>
    <ConnectionBuilder Name="AggregateIdentifierIsChildOfTargetAggregateIdentifiersBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="AggregateIdentifierIsChildOfTargetAggregateIdentifiers" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="AggregateIdentifier" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="AggregateIdentifier" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
    <ConnectionBuilder Name="ProjectionDefinitionHandlesEventDefinitionsBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="ProjectionDefinitionHandlesEventDefinitions" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="ProjectionDefinition" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="EventDefinition" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
    <ConnectionBuilder Name="QueryDefinitionReferencesIdentityGroupBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="QueryDefinitionReferencesIdentityGroup" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="QueryDefinition" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="IdentityGroup" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
    <ConnectionBuilder Name="IdentityGroupReferencesClassifierBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="IdentityGroupReferencesClassifier" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="IdentityGroup" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Classifier" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
    <ConnectionBuilder Name="ClassifierHandlesEventsBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="ClassifierHandlesEvents" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Classifier" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="EventDefinition" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
    <ConnectionBuilder Name="CommandDefinitionReferencesIdentityGroupBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="CommandDefinitionReferencesIdentityGroup" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="CommandDefinition" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="IdentityGroup" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
    <ConnectionBuilder Name="QueryDefinitionReferencesProjectionDefinitionBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="QueryDefinitionReferencesProjectionDefinition" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="QueryDefinition" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="ProjectionDefinition" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
  </ConnectionBuilders>
  <Diagram Id="07a5d33a-93dd-4c5d-bb00-dff3a44ecf56" Description="Architecture layout for a model based on the CQRS event sourcing architecture" Name="CQRSdslDiagram" DisplayName="CQRS Architecture diagram" Namespace="CQRSAzure.CQRSdsl.Dsl">
    <Properties>
      <DomainProperty Id="0755a035-f72c-43e2-87e3-875dca7e5f5f" Description="The language to use to generate code from this model" Name="OutputCodeLanguage" DisplayName="Output Code Language" DefaultValue="VBNet" Category="Code Generation">
        <Notes>This should be initialised from the language the project is hosted in but can be overriden if the code is going to be created in separate projects</Notes>
        <Type>
          <DomainEnumerationMoniker Name="TargetLanguage" />
        </Type>
      </DomainProperty>
      <DomainProperty Id="675490b0-d5e5-4d9e-8b3e-f8ed308e13ff" Description="Copyright notice to include in generated code and documentation" Name="CopyrightNotice" DisplayName="Copyright Notice" Category="Documentation">
        <Type>
          <ExternalTypeMoniker Name="/System/String" />
        </Type>
      </DomainProperty>
      <DomainProperty Id="cdc33a6f-e12f-4cc6-a5c3-7e2fde88d3cd" Description="Company name to embed in documentation and generated code" Name="CompanyName" DisplayName="Company Name" Category="Documentation">
        <Type>
          <ExternalTypeMoniker Name="/System/String" />
        </Type>
      </DomainProperty>
      <DomainProperty Id="1fca977f-2697-42e0-af61-fe29633c2081" Description="Description for CQRSAzure.CQRSdsl.Dsl.CQRSdslDiagram.Subfolder Per Model" Name="SubfolderPerModel" DisplayName="Subfolder Per Model" DefaultValue="true" Category="Code Generation">
        <Notes>Should each domain model go into its own subfolder?</Notes>
        <Type>
          <ExternalTypeMoniker Name="/System/Boolean" />
        </Type>
      </DomainProperty>
      <DomainProperty Id="ab1493fe-58ae-4ece-90c2-756ffbaeb661" Description="Description for CQRSAzure.CQRSdsl.Dsl.CQRSdslDiagram.Subfolder Per Aggregate" Name="SubfolderPerAggregate" DisplayName="Subfolder Per Aggregate" DefaultValue="true" Category="Code Generation">
        <Notes>Should each aggregate in the model be generated into its own subfolder</Notes>
        <Type>
          <ExternalTypeMoniker Name="/System/Boolean" />
        </Type>
      </DomainProperty>
    </Properties>
    <Class>
      <DomainClassMoniker Name="CQRSModel" />
    </Class>
    <ShapeMaps>
      <ShapeMap>
        <DomainClassMoniker Name="AggregateIdentifier" />
        <ParentElementPath>
          <DomainPath>CQRSModelHasAggregateIdentifiers.CQRSModel/!CQRSModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="AggregateGeometryShape/NameTextDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="AggregateIdentifier/Name" />
              <DomainPath />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="AggregateGeometryShape/DescriptionTextDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="AggregateIdentifier/Description" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <GeometryShapeMoniker Name="AggregateGeometryShape" />
      </ShapeMap>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="QueryDefinition" />
        <ParentElementPath>
          <DomainPath>AggregateIdentifierHasQueryDefinitions.AggregateIdentifier/!AggregateIdentifier/CQRSModelHasAggregateIdentifiers.CQRSModel/!CQRSModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="QueryDefinitionShape/NameTextDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="QueryDefinition/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="QueryDefinitionShape/DescriptionTextDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="QueryDefinition/Description" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="QueryDefinitionShape" />
        <CompartmentMap>
          <CompartmentMoniker Name="QueryDefinitionShape/InputParametersCompartment" />
          <ElementsDisplayed>
            <DomainPath>QueryDefinitionHasQueryInputParameters.QueryInputParameters/!QueryInputParameter</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="QueryInputParameter/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
        <CompartmentMap>
          <CompartmentMoniker Name="QueryDefinitionShape/OutputParametersCompartment" />
          <ElementsDisplayed>
            <DomainPath>QueryDefinitionHasQueryReturnParameters.QueryReturnParameters/!QueryReturnParameter</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="QueryReturnParameter/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
      </CompartmentShapeMap>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="EventDefinition" />
        <ParentElementPath>
          <DomainPath>AggregateIdentifierHasEventDefinitions.AggregateIdentifier/!AggregateIdentifier/CQRSModelHasAggregateIdentifiers.CQRSModel/!CQRSModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="EventDefinitionCompartmentShape/NameTextDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="EventDefinition/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="EventDefinitionCompartmentShape/DescriptionTextDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="EventDefinition/Description" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="EventDefinitionCompartmentShape/VersionNumberDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="EventDefinition/Version" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="EventDefinitionCompartmentShape" />
        <CompartmentMap>
          <CompartmentMoniker Name="EventDefinitionCompartmentShape/EventProperties" />
          <ElementsDisplayed>
            <DomainPath>EventDefinitionHasEventProperties.EventProperties/!EventProperty</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="EventProperty/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
      </CompartmentShapeMap>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="ProjectionDefinition" />
        <ParentElementPath>
          <DomainPath>AggregateIdentifierHasProjectionDefinitions.AggregateIdentifier/!AggregateIdentifier/CQRSModelHasAggregateIdentifiers.CQRSModel/!CQRSModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="ProjectionDefinitionCompartmentShape/NameTextDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="ProjectionDefinition/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="ProjectionDefinitionCompartmentShape/DescriptionTextDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="ProjectionDefinition/Description" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="ProjectionDefinitionCompartmentShape" />
        <CompartmentMap>
          <CompartmentMoniker Name="ProjectionDefinitionCompartmentShape/ProjectionProperties" />
          <ElementsDisplayed>
            <DomainPath>ProjectionDefinitionHasProjectionProperties.ProjectionProperties/!ProjectionProperty</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="ProjectionProperty/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
        <CompartmentMap DisplaysCustomString="true">
          <CompartmentMoniker Name="ProjectionDefinitionCompartmentShape/PropertyOperations" />
          <ElementsDisplayed>
            <DomainPath>ProjectionDefinitionHasEventPropertyOperations.ProjectionEventPropertyOperations/!ProjectionEventPropertyOperation</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="ProjectionEventPropertyOperation/EventName" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
      </CompartmentShapeMap>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="CommandDefinition" />
        <ParentElementPath>
          <DomainPath>AggregateIdentifierHasCommandDefinitions.AggregateIdentifier/!AggregateIdentifier/CQRSModelHasAggregateIdentifiers.CQRSModel/!CQRSModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="CommandDefinitionCompartmentShape/NameTextDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="CommandDefinition/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="CommandDefinitionCompartmentShape/DescriptionTextDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="CommandDefinition/Description" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="CommandDefinitionCompartmentShape" />
        <CompartmentMap>
          <CompartmentMoniker Name="CommandDefinitionCompartmentShape/PropertiesCompartment" />
          <ElementsDisplayed>
            <DomainPath>CommandDefinitionHasParameters.CommandParameters/!CommandParameter</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="CommandParameter/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
      </CompartmentShapeMap>
      <ShapeMap>
        <DomainClassMoniker Name="IdentityGroup" />
        <ParentElementPath>
          <DomainPath>AggregateIdentifierHasIdentityGrouped.AggregateIdentifier/!AggregateIdentifier/CQRSModelHasAggregateIdentifiers.CQRSModel/!CQRSModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="IdentityGroupGeometryShape/DescriptionTextDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="IdentityGroup/Description" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="IdentityGroupGeometryShape/NameTextDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="IdentityGroup/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <GeometryShapeMoniker Name="IdentityGroupGeometryShape" />
      </ShapeMap>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="Classifier" />
        <ParentElementPath>
          <DomainPath>AggregateIdentifierHasClassifiers.AggregateIdentifier/!AggregateIdentifier/CQRSModelHasAggregateIdentifiers.CQRSModel/!CQRSModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="ClassifierCompartmentShape/NameTextDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="Classifier/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="ClassifierCompartmentShape/DescriptionTextDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="Classifier/Notes" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="ClassifierCompartmentShape" />
        <CompartmentMap DisplaysCustomString="true">
          <CompartmentMoniker Name="ClassifierCompartmentShape/EventEvaluationsCompartment" />
          <ElementsDisplayed>
            <DomainPath>ClassifierHasEventEvaluations.ClassifierEventEvaluations/!ClassifierEventEvaluation</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="ClassifierEventEvaluation/Description" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
      </CompartmentShapeMap>
    </ShapeMaps>
    <ConnectorMaps>
      <ConnectorMap>
        <ConnectorMoniker Name="AggregateParenthoodConnector" />
        <DomainRelationshipMoniker Name="AggregateIdentifierIsChildOfTargetAggregateIdentifiers" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="AggregateEventConnector" />
        <DomainRelationshipMoniker Name="AggregateIdentifierHasEventDefinitions" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="AggregateProjectionConnector" />
        <DomainRelationshipMoniker Name="AggregateIdentifierHasProjectionDefinitions" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="AggregateQueryDefinitionConnector" />
        <DomainRelationshipMoniker Name="AggregateIdentifierHasQueryDefinitions" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="AggregateCommandDefinitionConnector" />
        <DomainRelationshipMoniker Name="AggregateIdentifierHasCommandDefinitions" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="ProjectionEventConnector" />
        <DomainRelationshipMoniker Name="ProjectionDefinitionHandlesEventDefinitions" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="AggregateIdentityGroupConnector" />
        <DomainRelationshipMoniker Name="AggregateIdentifierHasIdentityGrouped" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="IdentityGroupClassifierConnector" />
        <DomainRelationshipMoniker Name="IdentityGroupReferencesClassifier" />
      </ConnectorMap>
      <ConnectorMap>
        <Notes>Connection between a classifier and the events it handles</Notes>
        <ConnectorMoniker Name="ClassifierEventConnector" />
        <DomainRelationshipMoniker Name="ClassifierHandlesEvents" />
      </ConnectorMap>
    </ConnectorMaps>
  </Diagram>
  <Designer CopyPasteGeneration="CopyPasteOnly" FileExtension="cqrsx" EditorGuid="97bf9663-c5a5-48c4-b8a4-fa384db6ea5c">
    <RootClass>
      <DomainClassMoniker Name="CQRSModel" />
    </RootClass>
    <XmlSerializationDefinition CustomPostLoad="false">
      <XmlSerializationBehaviorMoniker Name="CQRSdslSerializationBehavior" />
    </XmlSerializationDefinition>
    <ToolboxTab TabText="CQRS Designer">
      <Notes>Tools to graphically design a CQRS backed application
</Notes>
      <ElementTool Name="AggregateTool" ToolboxIcon="Resources\AggregateShapeToolBitmap.bmp" Caption="Aggregate Identifier" Tooltip="Aggregate Identifier" HelpKeyword="Aggregate">
        <Notes>Create an aggregate identifier of every thing in the system for which we are tracking events</Notes>
        <DomainClassMoniker Name="AggregateIdentifier" />
      </ElementTool>
      <ElementTool Name="EventDefinitionTool" ToolboxIcon="Resources\EventShapeToolBitmap.bmp" Caption="Event Definition" Tooltip="Event Definition" HelpKeyword="EventDefinitionTool">
        <DomainClassMoniker Name="EventDefinition" />
      </ElementTool>
      <ElementTool Name="ProjectionTool" ToolboxIcon="Resources\ProjectionShapeToolBitmap.bmp" Caption="Projection Definition" Tooltip="Projection Definition" HelpKeyword="ProjectionTool">
        <DomainClassMoniker Name="ProjectionDefinition" />
      </ElementTool>
      <ElementTool Name="CommandTool" ToolboxIcon="Resources\CommandShapeToolBitmap.bmp" Caption="Command Definition" Tooltip="Command Definition" HelpKeyword="CommandTool">
        <Notes>Adds a command to the doamin for the specified aggregate identifier</Notes>
        <DomainClassMoniker Name="CommandDefinition" />
      </ElementTool>
      <ElementTool Name="QueryTool" ToolboxIcon="Resources\QueryShapeToolBitmap.bmp" Caption="Query Definition" Tooltip="Query Definition" HelpKeyword="QueryTool">
        <DomainClassMoniker Name="QueryDefinition" />
      </ElementTool>
      <ConnectionTool Name="ProjectionEventConnectionTool" ToolboxIcon="Resources\ProjectionEventConnectorToolBitmap.bmp" Caption="Projection Event" Tooltip="Projection Event Handler Connection" HelpKeyword="ProjectionEvent" SourceCursorIcon="Resources\ConnectorSourceSearch.cur" TargetCursorIcon="Resources\ConnectorTargetSearch.cur">
        <ConnectionBuilderMoniker Name="CQRSdsl/ProjectionDefinitionHandlesEventDefinitionsBuilder" />
      </ConnectionTool>
      <ConnectionTool Name="AggregateParenthoodConnectionTool" ToolboxIcon="Resources\AggregateParentConnectorToolBitmap.bmp" Caption="Aggregate Hierarchy" Tooltip="Aggregate Hierarchy Connection" HelpKeyword="AggregateParenthoodConnectionTool" SourceCursorIcon="Resources\ConnectorSourceSearch.cur" TargetCursorIcon="Resources\ConnectorTargetSearch.cur">
        <Notes>Allows aggregates to be linked in a hierarchy</Notes>
        <ConnectionBuilderMoniker Name="CQRSdsl/AggregateIdentifierIsChildOfTargetAggregateIdentifiersBuilder" />
      </ConnectionTool>
      <ElementTool Name="IdentityGroupTool" ToolboxIcon="Resources\IdentityGroupToolBitmap.bmp" Caption="Identity Group" Tooltip="Identity Group Tool" HelpKeyword="IdentityGroup">
        <DomainClassMoniker Name="IdentityGroup" />
      </ElementTool>
      <ElementTool Name="ClassifierTool" ToolboxIcon="Resources\IdentityGroupToolBitmap.bmp" Caption="Classifier" Tooltip="Classifier" HelpKeyword="ClassifierTool">
        <Notes>A class that classifies membership of an identity group</Notes>
        <DomainClassMoniker Name="Classifier" />
      </ElementTool>
      <ConnectionTool Name="ClassifierEventConnectionTool" ToolboxIcon="Resources\ProjectionEventConnectorToolBitmap.bmp" Caption="Classifier Event Connection" Tooltip="Classifier Event Connection Tool" HelpKeyword="ClassifierEventConnection">
        <Notes>The events that are processed by the classifier to evaluate group membership</Notes>
        <ConnectionBuilderMoniker Name="CQRSdsl/ClassifierHandlesEventsBuilder" />
      </ConnectionTool>
    </ToolboxTab>
    <Validation UsesMenu="true" UsesOpen="false" UsesSave="true" UsesLoad="true" />
    <DiagramMoniker Name="CQRSdslDiagram" />
  </Designer>
  <Explorer ExplorerGuid="115f2380-e1f5-418c-bcb8-bedf1ce58a28" Title="CQRS Domain Explorer">
    <ExplorerBehaviorMoniker Name="CQRSdsl/CQRSdslExplorer" />
  </Explorer>
</Dsl>