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
  </typeDefinitions>
  <configurationElements>
    <configurationSection name="WeTextConfiguration" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="weTextConfiguration">
      <elementProperties>
        <elementProperty name="CommandSenderSettings" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="commandSender" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/64189409-4a3e-47e0-92c2-7c7c92b4ed19/SettingElementCollection" />
          </type>
        </elementProperty>
        <elementProperty name="EventPublisherSettings" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="eventPublisher" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/64189409-4a3e-47e0-92c2-7c7c92b4ed19/SettingElementCollection" />
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
    <configurationElementCollection name="ServiceElementCollection" xmlItemName="service" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
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
      </attributeProperties>
      <elementProperties>
        <elementProperty name="Settings" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="settings" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/64189409-4a3e-47e0-92c2-7c7c92b4ed19/SettingElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElement name="ApplicationSettingElement">
      <attributeProperties>
        <attributeProperty name="Url" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="url" isReadOnly="false">
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