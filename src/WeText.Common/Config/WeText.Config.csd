<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="64189409-4a3e-47e0-92c2-7c7c92b4ed19" namespace="WeText.Common.Config" xmlSchemaNamespace="urn:WeText.Common.Config" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
  <typeDefinitions>
    <externalType name="String" namespace="System" />
    <externalType name="Boolean" namespace="System" />
    <externalType name="Int32" namespace="System" />
    <externalType name="Int64" namespace="System" />
    <externalType name="Single" namespace="System" />
    <externalType name="Double" namespace="System" />
    <externalType name="DateTime" namespace="System" />
    <externalType name="TimeSpan" namespace="System" />
    <enumeratedType name="MessageQueueResourceType" namespace="WeText.Common.Config">
      <literals>
        <enumerationLiteral name="MessageExchange" />
        <enumerationLiteral name="MessageQueue" />
      </literals>
    </enumeratedType>
  </typeDefinitions>
  <configurationElements>
    <configurationSection name="WeTextConfiguration" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="weTextConfiguration">
      <elementProperties>
        <elementProperty name="CommandQueue" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="commandQueue" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/64189409-4a3e-47e0-92c2-7c7c92b4ed19/MessageQueueConfigurationElement" />
          </type>
        </elementProperty>
        <elementProperty name="EventQueue" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="eventQueue" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/64189409-4a3e-47e0-92c2-7c7c92b4ed19/MessageQueueConfigurationElement" />
          </type>
        </elementProperty>
        <elementProperty name="Services" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="services" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/64189409-4a3e-47e0-92c2-7c7c92b4ed19/ServiceElementCollection" />
          </type>
        </elementProperty>
        <elementProperty name="ApplicationSetting" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="application" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/64189409-4a3e-47e0-92c2-7c7c92b4ed19/ApplicationSettingElement" />
          </type>
        </elementProperty>
        <elementProperty name="MongoEventStore" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="mongoEventStore" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/64189409-4a3e-47e0-92c2-7c7c92b4ed19/MongoEventStoreElement" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElement name="SettingElement">
      <attributeProperties>
        <attributeProperty name="Key" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="key" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/64189409-4a3e-47e0-92c2-7c7c92b4ed19/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Value" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="value" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/64189409-4a3e-47e0-92c2-7c7c92b4ed19/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="SettingElementCollection" xmlItemName="setting" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/64189409-4a3e-47e0-92c2-7c7c92b4ed19/SettingElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElementCollection name="ServiceElementCollection" xmlItemName="service" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods, ICollection">
      <itemType>
        <configurationElementMoniker name="/64189409-4a3e-47e0-92c2-7c7c92b4ed19/ServiceElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="ServiceElement">
      <attributeProperties>
        <attributeProperty name="Type" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="type" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/64189409-4a3e-47e0-92c2-7c7c92b4ed19/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="InstanceId" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="instanceId" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/64189409-4a3e-47e0-92c2-7c7c92b4ed19/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="Settings" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="settings" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/64189409-4a3e-47e0-92c2-7c7c92b4ed19/SettingElementCollection" />
          </type>
        </elementProperty>
        <elementProperty name="LocalCommandQueue" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="localCommandQueue" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/64189409-4a3e-47e0-92c2-7c7c92b4ed19/MessageQueueConfigurationElement" />
          </type>
        </elementProperty>
        <elementProperty name="LocalEventQueue" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="localEventQueue" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/64189409-4a3e-47e0-92c2-7c7c92b4ed19/MessageQueueConfigurationElement" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElement name="ApplicationSettingElement">
      <attributeProperties>
        <attributeProperty name="Url" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="url" isReadOnly="false" documentation="Represents the base url of the WeText service.">
          <type>
            <externalTypeMoniker name="/64189409-4a3e-47e0-92c2-7c7c92b4ed19/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElement name="MessageQueueConfigurationElement">
      <attributeProperties>
        <attributeProperty name="ConnectionUri" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="connectionUri" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/64189409-4a3e-47e0-92c2-7c7c92b4ed19/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="ExchangeName" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="exchangeName" isReadOnly="false" documentation="Gets or sets the name of the resource of the message queue. A resource can be either a queue, or an exchange to which queues can be bound.">
          <type>
            <externalTypeMoniker name="/64189409-4a3e-47e0-92c2-7c7c92b4ed19/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="QueueName" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="queueName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/64189409-4a3e-47e0-92c2-7c7c92b4ed19/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElement name="MongoEventStoreElement">
      <attributeProperties>
        <attributeProperty name="ConnectionString" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="connectionString" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/64189409-4a3e-47e0-92c2-7c7c92b4ed19/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Database" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="database" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/64189409-4a3e-47e0-92c2-7c7c92b4ed19/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="CollectionName" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="collectionName" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/64189409-4a3e-47e0-92c2-7c7c92b4ed19/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>