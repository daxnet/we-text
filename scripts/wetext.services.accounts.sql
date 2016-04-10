CREATE DATABASE `wetext` /*!40100 DEFAULT CHARACTER SET utf8 */;
CREATE TABLE `accounts` (
  `Id` varchar(64) NOT NULL,
  `Name` varchar(16) NOT NULL,
  `Email` varchar(32) NOT NULL,
  `Password` varchar(16) NOT NULL,
  `DisplayName` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  UNIQUE KEY `Email_UNIQUE` (`Email`),
  UNIQUE KEY `Name_UNIQUE` (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
