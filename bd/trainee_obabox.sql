-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema trainee_obabox
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema trainee_obabox
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `trainee_obabox` DEFAULT CHARACTER SET utf8 ;
USE `trainee_obabox` ;

-- -----------------------------------------------------
-- Table `trainee_obabox`.`loja`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `trainee_obabox`.`loja` ;

CREATE TABLE IF NOT EXISTS `trainee_obabox`.`loja` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `nome` NVARCHAR(60) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `trainee_obabox`.`cliente`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `trainee_obabox`.`cliente` ;

CREATE TABLE IF NOT EXISTS `trainee_obabox`.`cliente` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `nome` NVARCHAR(60) NOT NULL,
  `cpf` NVARCHAR(14) NOT NULL,
  `rg` NVARCHAR(20) NOT NULL,
  `dt_nasc` DATETIME NOT NULL,
  `ativo` BIT NULL DEFAULT 1,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `trainee_obabox`.`endereco`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `trainee_obabox`.`endereco` ;

CREATE TABLE IF NOT EXISTS `trainee_obabox`.`endereco` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `cliente_id` INT UNSIGNED NOT NULL,
  `logradouro` NVARCHAR(50) NOT NULL,
  `numero` NVARCHAR(15) NOT NULL,
  `complemento` NVARCHAR(40) NOT NULL,
  `cidade` NVARCHAR(40) NOT NULL,
  `uf` NVARCHAR(2) NOT NULL,
  `cep` NVARCHAR(10) NOT NULL,
  PRIMARY KEY (`id`, `cliente_id`),
  INDEX `fk_endereco_cliente1_idx` (`cliente_id` ASC),
  CONSTRAINT `fk_endereco_cliente1`
    FOREIGN KEY (`cliente_id`)
    REFERENCES `trainee_obabox`.`cliente` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `trainee_obabox`.`loja_has_cliente`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `trainee_obabox`.`loja_has_cliente` ;

CREATE TABLE IF NOT EXISTS `trainee_obabox`.`loja_has_cliente` (
  `loja_id` INT UNSIGNED NOT NULL,
  `cliente_id` INT UNSIGNED NOT NULL,
  `endereco_id` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`loja_id`, `cliente_id`, `endereco_id`),
  INDEX `fk_loja_has_cliente_cliente1_idx` (`cliente_id` ASC),
  INDEX `fk_loja_has_cliente_loja_idx` (`loja_id` ASC),
  INDEX `fk_loja_has_cliente_endereco1_idx` (`endereco_id` ASC),
  CONSTRAINT `fk_loja_has_cliente_loja`
    FOREIGN KEY (`loja_id`)
    REFERENCES `trainee_obabox`.`loja` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_loja_has_cliente_cliente1`
    FOREIGN KEY (`cliente_id`)
    REFERENCES `trainee_obabox`.`cliente` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_loja_has_cliente_endereco1`
    FOREIGN KEY (`endereco_id`)
    REFERENCES `trainee_obabox`.`endereco` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

USE `trainee_obabox` ;

-- -----------------------------------------------------
-- Placeholder table for view `trainee_obabox`.`verCompras`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `trainee_obabox`.`verCompras` (`nome_loja` INT, `loja_id` INT, `nome_cliente` INT, `cliente_id` INT, `endereco` INT, `endereco_id` INT);

-- -----------------------------------------------------
-- Placeholder table for view `trainee_obabox`.`verEnderecos`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `trainee_obabox`.`verEnderecos` (`id` INT, `cliente` INT, `endereco` INT);

-- -----------------------------------------------------
-- View `trainee_obabox`.`verCompras`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `trainee_obabox`.`verCompras`;
DROP VIEW IF EXISTS `trainee_obabox`.`verCompras` ;
USE `trainee_obabox`;
CREATE  OR REPLACE VIEW `verCompras` AS
SELECT
    l.nome AS nome_loja,
    l.id AS loja_id,
    c.nome AS nome_cliente,
    c.id AS cliente_id,
    CONCAT(e.logradouro,' ',e.numero,' - ',e.cidade) AS endereco,
    e.id AS endereco_id
FROM
loja_has_cliente lc
JOIN loja l ON lc.loja_id = l.id
JOIN cliente c ON lc.cliente_id = c.id
JOIN endereco e ON lc.endereco_id = e.id;

-- -----------------------------------------------------
-- View `trainee_obabox`.`verEnderecos`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `trainee_obabox`.`verEnderecos`;
DROP VIEW IF EXISTS `trainee_obabox`.`verEnderecos` ;
USE `trainee_obabox`;
CREATE  OR REPLACE VIEW `verEnderecos` AS
SELECT 
e.id,
c.nome as cliente,
CONCAT(`e`.`logradouro`,' ',`e`.`numero`,' - ',`e`.`cidade`) AS `endereco`
FROM `endereco` e
JOIN cliente c ON e.cliente_id = c.id;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

-- -----------------------------------------------------
-- Data for table `trainee_obabox`.`loja`
-- -----------------------------------------------------
START TRANSACTION;
USE `trainee_obabox`;
INSERT INTO `trainee_obabox`.`loja` (`id`, `nome`) VALUES (DEFAULT, 'Lojas Renner');
INSERT INTO `trainee_obabox`.`loja` (`id`, `nome`) VALUES (DEFAULT, 'Ricardo Eletro');
INSERT INTO `trainee_obabox`.`loja` (`id`, `nome`) VALUES (DEFAULT, 'Lojas Americanas');
INSERT INTO `trainee_obabox`.`loja` (`id`, `nome`) VALUES (DEFAULT, 'Ponto Frio');

COMMIT;

