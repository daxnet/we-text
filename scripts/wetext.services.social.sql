CREATE DATABASE `wetext.social` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `wetext.social`;
CREATE TABLE `usernames` (
  `UserId` varchar(64) NOT NULL,
  `DisplayName` varchar(32) NOT NULL,
  PRIMARY KEY (`UserId`),
  UNIQUE KEY `UserId_UNIQUE` (`UserId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
