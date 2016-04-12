CREATE DATABASE `wetext.texting` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `wetext.texting`;
CREATE TABLE `texts` (
  `Id` varchar(64) NOT NULL,
  `UserId` varchar(64) NOT NULL,
  `Title` varchar(32) NOT NULL,
  `Content` longtext NOT NULL,
  `DateCreated` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;



