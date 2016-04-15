CREATE DATABASE `wetext.social` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `wetext.social`;
CREATE TABLE `networks` (
  `OriginatorId` varchar(64) NOT NULL,
  `TargetId` varchar(64) NOT NULL,
  `InvitationStartDate` datetime NOT NULL,
  `InvitationEndDate` datetime(6) DEFAULT NULL,
  `InvitationEndReason` int(11) DEFAULT NULL,
  `InvitationId` varchar(64) NOT NULL,
  `OriginatorName` varchar(32) NOT NULL,
  `TargetUserName` varchar(32) NOT NULL,
  PRIMARY KEY (`InvitationId`),
  UNIQUE KEY `InvitationId_UNIQUE` (`InvitationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
CREATE TABLE `usernames` (
  `UserId` varchar(64) NOT NULL,
  `DisplayName` varchar(32) NOT NULL,
  PRIMARY KEY (`UserId`),
  UNIQUE KEY `UserId_UNIQUE` (`UserId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

