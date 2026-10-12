-- MySQL dump 10.13  Distrib 8.0.46, for Win64 (x86_64)
--
-- Host: localhost    Database: fleet_management
-- ------------------------------------------------------
-- Server version	8.0.46

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20261012072253_InitialCreate','8.0.13');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `refresh_tokens`
--

DROP TABLE IF EXISTS `refresh_tokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `refresh_tokens` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `user_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `token_hash` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `jti` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `expires_at` datetime(6) NOT NULL,
  `revoked_at` datetime(6) DEFAULT NULL,
  `replaced_by` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `user_agent` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ip_address` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `created_at` datetime(6) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `IX_refresh_tokens_token_hash` (`token_hash`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `refresh_tokens`
--

LOCK TABLES `refresh_tokens` WRITE;
/*!40000 ALTER TABLE `refresh_tokens` DISABLE KEYS */;
INSERT INTO `refresh_tokens` VALUES (1,'139856b2-0cc7-4307-b758-45c9ba408444','235b813954d343855fcc64d2e4bdb96a7e0b07034caf52b3edfb9d94ffe1e38d','dc2b09b9-7e1e-4b80-95ce-77e3a94d1f73','2026-10-19 07:45:13.347114','2026-10-12 07:45:17.499023',NULL,'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/154.0.0.0 Safari/537.36','::1','2026-10-12 07:45:13.347268'),(2,'139856b2-0cc7-4307-b758-45c9ba408444','3a545f1586e621ef76b4613ad0369d335d7047eaed61dc1554ff52ab2c7b34ef','f58dfae5-a56e-474a-96d0-05a555059bf6','2026-10-19 07:45:37.819076','2026-10-12 07:45:39.583554',NULL,'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/154.0.0.0 Safari/537.36','::1','2026-10-12 07:45:37.819077'),(3,'139856b2-0cc7-4307-b758-45c9ba408444','e0a18a927c3a6cbed6bd7e249e97f1807a67c46e690cc3834ea9904f61602df6','280e3ce7-86d7-4eda-a330-26dc7197c964','2026-10-19 07:55:17.793174','2026-10-12 07:56:28.811850',NULL,'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/154.0.0.0 Safari/537.36','::1','2026-10-12 07:55:17.793262'),(4,'b657b9c2-8c11-42ec-bb8d-22f426cce88b','35cb41c20a102aa615d8ffd206a8e516254c733f2252fa1a4b0c9c19257a5e10','91f995f1-4793-4d0a-bed4-4794d86111bc','2026-10-19 07:56:37.482115','2026-10-12 07:56:42.021564',NULL,'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/154.0.0.0 Safari/537.36','::1','2026-10-12 07:56:37.482115'),(5,'139856b2-0cc7-4307-b758-45c9ba408444','d4ce2b4d6124678c13b926a96cdb27cc41971b9e4cab2d40e42cb0a669fc2b74','618117df-d593-4892-b630-fa590680326b','2026-10-19 07:58:52.261344',NULL,NULL,'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/154.0.0.0 Safari/537.36','::1','2026-10-12 07:58:52.261344'),(6,'139856b2-0cc7-4307-b758-45c9ba408444','6ca5b73bf0a90e385fd896d90d5adddb6d12522dbc120ab7fa00b492db99fbac','668216a9-640b-4903-aed4-c3ce463de2d2','2026-10-19 09:03:56.426122','2026-10-12 09:04:16.212490',NULL,'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/154.0.0.0 Safari/537.36','::1','2026-10-12 09:03:56.426208'),(7,'b657b9c2-8c11-42ec-bb8d-22f426cce88b','55f5d5e7a844a4cdacb44bb461f30d7df96862f14adb33b708d6529189279391','588daaac-c30f-4ac2-953f-c30c2db76266','2026-10-19 09:04:20.565154','2026-10-12 09:08:49.478637',NULL,'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/154.0.0.0 Safari/537.36','::1','2026-10-12 09:04:20.565155'),(8,'b657b9c2-8c11-42ec-bb8d-22f426cce88b','6632430c5c09c74bf7a18d16bd4cba48e0fae59409c819279c6997d3eab8d1d4','25153a1e-363f-46b8-a56f-18b4d18a6779','2026-10-19 09:08:54.312442',NULL,NULL,'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/154.0.0.0 Safari/537.36','::1','2026-10-12 09:08:54.312534');
/*!40000 ALTER TABLE `refresh_tokens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `role_claims`
--

DROP TABLE IF EXISTS `role_claims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `role_claims` (
  `id` int NOT NULL AUTO_INCREMENT,
  `role_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `claim_type` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `claim_value` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_role_claims_role_id` (`role_id`),
  CONSTRAINT `FK_role_claims_roles_role_id` FOREIGN KEY (`role_id`) REFERENCES `roles` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `role_claims`
--

LOCK TABLES `role_claims` WRITE;
/*!40000 ALTER TABLE `role_claims` DISABLE KEYS */;
/*!40000 ALTER TABLE `role_claims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `roles`
--

DROP TABLE IF EXISTS `roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `roles` (
  `id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `description` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `created_at` datetime(6) NOT NULL,
  `name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `normalized_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `concurrency_stamp` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `RoleNameIndex` (`normalized_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `roles`
--

LOCK TABLES `roles` WRITE;
/*!40000 ALTER TABLE `roles` DISABLE KEYS */;
INSERT INTO `roles` VALUES ('21ddad49-49cd-46fa-9cc0-7cff8c968582','Driver access','2026-10-12 07:24:57.942906','driver','DRIVER',NULL),('7c11d77c-8914-4697-bb5d-5bdd6ba2a97e','Fleet operations management','2026-10-12 07:24:57.942902','manager','MANAGER',NULL),('8c97d7c2-3732-47a2-bc69-283aa8c386a2','Full system access','2026-10-12 07:24:57.942790','admin','ADMIN',NULL),('c8a09a83-3f5e-497f-876a-4ca8abe2062b','Read only','2026-10-12 07:24:57.942907','viewer','VIEWER',NULL);
/*!40000 ALTER TABLE `roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `token_blacklist`
--

DROP TABLE IF EXISTS `token_blacklist`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `token_blacklist` (
  `jti` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `user_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `expires_at` datetime(6) NOT NULL,
  `reason` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `created_at` datetime(6) NOT NULL,
  PRIMARY KEY (`jti`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `token_blacklist`
--

LOCK TABLES `token_blacklist` WRITE;
/*!40000 ALTER TABLE `token_blacklist` DISABLE KEYS */;
INSERT INTO `token_blacklist` VALUES ('06e502ea-5846-4f5d-b656-b49c8fa50090','b657b9c2-8c11-42ec-bb8d-22f426cce88b','2026-10-12 08:11:37.000000','logout','2026-10-12 07:56:42.027102'),('6e8bc01b-50ee-4d35-a2a6-6cdb5974222a','139856b2-0cc7-4307-b758-45c9ba408444','2026-10-12 08:10:17.000000','logout','2026-10-12 07:56:28.831614'),('70e6f467-f7ec-40c6-b992-c4279b5cce73','139856b2-0cc7-4307-b758-45c9ba408444','2026-10-12 09:18:56.000000','logout','2026-10-12 09:04:16.222145'),('77969b2c-78dd-4d1e-9cee-6b024d16b89d','139856b2-0cc7-4307-b758-45c9ba408444','2026-10-12 08:00:13.000000','logout','2026-10-12 07:45:17.508449'),('ba88e0ca-6847-4fef-9534-c91b1181c307','b657b9c2-8c11-42ec-bb8d-22f426cce88b','2026-10-12 09:19:20.000000','logout','2026-10-12 09:08:49.643260'),('d922aad8-ebd7-4822-81e3-3a07fcb32278','139856b2-0cc7-4307-b758-45c9ba408444','2026-10-12 08:00:37.000000','logout','2026-10-12 07:45:39.587745');
/*!40000 ALTER TABLE `token_blacklist` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_claims`
--

DROP TABLE IF EXISTS `user_claims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_claims` (
  `id` int NOT NULL AUTO_INCREMENT,
  `user_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `claim_type` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `claim_value` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_user_claims_user_id` (`user_id`),
  CONSTRAINT `FK_user_claims_users_user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_claims`
--

LOCK TABLES `user_claims` WRITE;
/*!40000 ALTER TABLE `user_claims` DISABLE KEYS */;
/*!40000 ALTER TABLE `user_claims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_logins`
--

DROP TABLE IF EXISTS `user_logins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_logins` (
  `login_provider` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `provider_key` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `provider_display_name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `user_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`login_provider`,`provider_key`),
  KEY `IX_user_logins_user_id` (`user_id`),
  CONSTRAINT `FK_user_logins_users_user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_logins`
--

LOCK TABLES `user_logins` WRITE;
/*!40000 ALTER TABLE `user_logins` DISABLE KEYS */;
/*!40000 ALTER TABLE `user_logins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_roles`
--

DROP TABLE IF EXISTS `user_roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_roles` (
  `user_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `role_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`user_id`,`role_id`),
  KEY `IX_user_roles_role_id` (`role_id`),
  CONSTRAINT `FK_user_roles_roles_role_id` FOREIGN KEY (`role_id`) REFERENCES `roles` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_user_roles_users_user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_roles`
--

LOCK TABLES `user_roles` WRITE;
/*!40000 ALTER TABLE `user_roles` DISABLE KEYS */;
INSERT INTO `user_roles` VALUES ('2af83d0e-25a2-4873-abab-ec507a0eec17','21ddad49-49cd-46fa-9cc0-7cff8c968582'),('b657b9c2-8c11-42ec-bb8d-22f426cce88b','7c11d77c-8914-4697-bb5d-5bdd6ba2a97e'),('cc4ecdb4-a209-4991-af6a-5a8d7e08bce9','7c11d77c-8914-4697-bb5d-5bdd6ba2a97e'),('139856b2-0cc7-4307-b758-45c9ba408444','8c97d7c2-3732-47a2-bc69-283aa8c386a2');
/*!40000 ALTER TABLE `user_roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_tokens`
--

DROP TABLE IF EXISTS `user_tokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_tokens` (
  `user_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `login_provider` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `name` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `value` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`user_id`,`login_provider`,`name`),
  CONSTRAINT `FK_user_tokens_users_user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_tokens`
--

LOCK TABLES `user_tokens` WRITE;
/*!40000 ALTER TABLE `user_tokens` DISABLE KEYS */;
/*!40000 ALTER TABLE `user_tokens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `is_active` tinyint(1) NOT NULL,
  `last_login_at` datetime(6) DEFAULT NULL,
  `last_login_ip` varchar(45) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `created_at` datetime(6) NOT NULL,
  `updated_at` datetime(6) NOT NULL,
  `deleted_at` datetime(6) DEFAULT NULL,
  `user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `normalized_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `email` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `normalized_email` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `email_confirmed` tinyint(1) NOT NULL,
  `password_hash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `security_stamp` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `concurrency_stamp` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `phone` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `phone_number_confirmed` tinyint(1) NOT NULL,
  `two_factor_enabled` tinyint(1) NOT NULL,
  `lockout_until` datetime(6) DEFAULT NULL,
  `lockout_enabled` tinyint(1) NOT NULL,
  `failed_attempts` int NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `UserNameIndex` (`normalized_user_name`),
  KEY `EmailIndex` (`normalized_email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES ('139856b2-0cc7-4307-b758-45c9ba408444','Super Admin',1,'2026-10-12 09:03:56.172218','::1','2026-10-12 07:25:01.113333','2026-10-12 07:25:01.113376',NULL,'admin@fleet.local','ADMIN@FLEET.LOCAL','admin@fleet.local','ADMIN@FLEET.LOCAL',1,'AQAAAAIAAYagAAAAECI0RWwoGLHuj8asAF6YjQ37N/I281DO5LaVvoCCKOejbSD3zjCqW0HOnhkHsoRoKQ==','3OG3PCJIMMRQHIZUGBOHW3FPHEAAKBWL','8a6671b1-33f4-4f42-9fba-abf5264cf21a','9999999999',0,0,NULL,1,0),('2af83d0e-25a2-4873-abab-ec507a0eec17','Ravi Kumar',1,NULL,NULL,'2026-10-12 07:25:01.839870','2026-10-12 07:25:01.839870',NULL,'ravi.driver@fleet.local','RAVI.DRIVER@FLEET.LOCAL','ravi.driver@fleet.local','RAVI.DRIVER@FLEET.LOCAL',1,'AQAAAAIAAYagAAAAEDRzsz10BTFpCJm2LqB7F1LK+2JtVFH0v7a3gaO7kWIM99Zw/MiszRiALtmYw7Uqqw==','WT6XHREV2QMYVOBWNVF424OFWO4WM6O7','35d42fb0-ff2a-48f2-a850-1dd65437436f','8888888888',0,0,NULL,1,2),('b657b9c2-8c11-42ec-bb8d-22f426cce88b','Ashish Raj Kuril',1,'2026-10-12 09:08:54.246581','::1','2026-10-12 07:56:20.787475','2026-10-12 07:56:20.787475',NULL,'ashishrajlife@gmail.com','ASHISHRAJLIFE@GMAIL.COM','ashishrajlife@gmail.com','ASHISHRAJLIFE@GMAIL.COM',1,'AQAAAAIAAYagAAAAEBzi2D8uFn3Wdmm7w4NdT3nP8nABgUZlJX3amm5dHYKURePiEOSu9we8XWP3mQB/cw==','DG24NTKEK6ADEE5WRSWKXQX55UFEXYIX','7391cf6d-1c53-4e0b-9664-56ac16dee671','09340930641',0,0,NULL,1,0),('cc4ecdb4-a209-4991-af6a-5a8d7e08bce9','Ops Manager',1,NULL,NULL,'2026-10-12 07:25:01.612528','2026-10-12 07:25:01.612528',NULL,'manager@fleet.local','MANAGER@FLEET.LOCAL','manager@fleet.local','MANAGER@FLEET.LOCAL',1,'AQAAAAIAAYagAAAAEOrpn+gDqL3KzApNRchwtT50roSndUPwBBP2PGpDyjhvtqc4tiaXR95d1w9rmUgGiQ==','YLFLRIEOVXFA5U3CVSIXAP45JEPAGXFJ','4e47f5cc-c836-4d58-a4ef-985eedb766d2','9999999998',0,0,'2026-10-12 08:01:41.047639',1,0);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-10-12 14:42:48
