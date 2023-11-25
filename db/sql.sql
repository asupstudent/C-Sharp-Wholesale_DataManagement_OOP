/******************************************************************************/
/***          Generated by IBExpert 2022.3.4.1 31.05.2023 18:47:59          ***/
/******************************************************************************/

SET SQL DIALECT 3;

SET NAMES UTF8;

CREATE DATABASE 'C:\Users\admin\source\repos\Wholesale\db\WHOLESALE.FDB'
USER 'SYSDBA' PASSWORD 'masterkey'
PAGE_SIZE 16384
DEFAULT CHARACTER SET UTF8 COLLATION UTF8;



/******************************************************************************/
/***                               Generators                               ***/
/******************************************************************************/

CREATE GENERATOR GEN_INVOICE_HEADER_ID START WITH 1 INCREMENT BY 1;
SET GENERATOR GEN_INVOICE_HEADER_ID TO 127;



/******************************************************************************/
/***                                 Tables                                 ***/
/******************************************************************************/



CREATE TABLE CATEGORY (
    ID    INTEGER GENERATED BY DEFAULT AS IDENTITY,
    NAME  VARCHAR(50)
);

CREATE TABLE CONTRACTOR (
    ID              INTEGER GENERATED BY DEFAULT AS IDENTITY,
    NAME            VARCHAR(100),
    ACTUAL_ADDRESS  VARCHAR(100),
    LEGAL_ADDRESS   VARCHAR(100),
    PHONE           VARCHAR(30)
);

CREATE TABLE EMPLOYEE (
    ID    INTEGER GENERATED BY DEFAULT AS IDENTITY,
    FIO   VARCHAR(50),
    POST  VARCHAR(50)
);

CREATE TABLE INVOICE_HEADER (
    ID               INTEGER NOT NULL,
    ID_CONTRACTOR    INTEGER,
    ID_EMPLOYEE      INTEGER,
    ID_INVOICE_TYPE  INTEGER,
    INVOICE_DATE     DATE
);

CREATE TABLE INVOICE_TABLE_PART (
    ID                 INTEGER GENERATED BY DEFAULT AS IDENTITY,
    ID_INVOICE_HEADER  INTEGER,
    ID_PRODUCT         INTEGER,
    AMOUNT             INTEGER,
    TOTAL_PRICE        DECIMAL(10,2),
    ID_WAREHOUSE       INTEGER
);

CREATE TABLE INVOICE_TYPE (
    ID    INTEGER GENERATED BY DEFAULT AS IDENTITY,
    NAME  VARCHAR(30)
);

CREATE TABLE PRODUCT (
    ID           INTEGER GENERATED BY DEFAULT AS IDENTITY,
    NAME         VARCHAR(100),
    TRADEMARK    VARCHAR(50),
    ID_CATEGORY  INTEGER,
    PRICE        DECIMAL(10,2),
    UNIT         VARCHAR(15),
    AMOUNT       INTEGER,
    MIN_STOCK    INTEGER,
    WANT_STOCK   INTEGER,
    DESCRIPTION  VARCHAR(1024)
);

CREATE TABLE WAREHOUSE (
    ID     INTEGER GENERATED BY DEFAULT AS IDENTITY,
    LINE   INTEGER,
    SHELF  INTEGER,
    PLACE  INTEGER
);

INSERT INTO CATEGORY (ID, NAME) VALUES (1, 'Овощи');
INSERT INTO CATEGORY (ID, NAME) VALUES (2, 'Фрукты');
INSERT INTO CATEGORY (ID, NAME) VALUES (3, 'Ягоды');


COMMIT WORK;

INSERT INTO CONTRACTOR (ID, NAME, ACTUAL_ADDRESS, LEGAL_ADDRESS, PHONE) VALUES (1, 'Агат', 'г. Москва ул. Строителей д. 25', 'г. Москва ул. Строителей д. 25', '89000000000');
INSERT INTO CONTRACTOR (ID, NAME, ACTUAL_ADDRESS, LEGAL_ADDRESS, PHONE) VALUES (2, 'Русполимет', 'г. Кулебаки ул. Восстания д. 1', 'г. Кулебаки ул. Восстания д. 1', NULL);
INSERT INTO CONTRACTOR (ID, NAME, ACTUAL_ADDRESS, LEGAL_ADDRESS, PHONE) VALUES (3, 'ЛД Автограф Клуб', 'г. Санкт-Петербург Петергофское шоссе д. 71', 'г. Санкт-Петербург Петергофское шоссе д. 71', NULL);
INSERT INTO CONTRACTOR (ID, NAME, ACTUAL_ADDRESS, LEGAL_ADDRESS, PHONE) VALUES (4, 'Сибирская Губерния', 'г. Новосибирск ул. Пионеров д. 34', 'г. Новосибирск ул. Пионеров д. 34', NULL);
INSERT INTO CONTRACTOR (ID, NAME, ACTUAL_ADDRESS, LEGAL_ADDRESS, PHONE) VALUES (5, 'Ивановская фабрика', 'г. Иваново ул. Мира д. 44', 'г. Иваново ул. Мира д. 44', NULL);
INSERT INTO CONTRACTOR (ID, NAME, ACTUAL_ADDRESS, LEGAL_ADDRESS, PHONE) VALUES (11, 'Веселая капуста', 'г. Ковров, ул. Зеленая, д. 55', 'г. Ковров, ул. Зеленая, д. 55', '588825');


COMMIT WORK;

INSERT INTO EMPLOYEE (ID, FIO, POST) VALUES (1, 'Скофф Иван Иванович', 'Директор');
INSERT INTO EMPLOYEE (ID, FIO, POST) VALUES (2, 'Волков Георгий Билалович', 'Кладовщик');
INSERT INTO EMPLOYEE (ID, FIO, POST) VALUES (3, 'Никифоров Матвей Александрович', 'Менеджер');
INSERT INTO EMPLOYEE (ID, FIO, POST) VALUES (4, 'Миронов Кирилл Константинович', 'Директор');
INSERT INTO EMPLOYEE (ID, FIO, POST) VALUES (16, 'Фролов Сергей Петрович', 'Сторож');
INSERT INTO EMPLOYEE (ID, FIO, POST) VALUES (19, 'Рябов Константин Николаевич', 'Техник');
INSERT INTO EMPLOYEE (ID, FIO, POST) VALUES (7, 'Мельников Артемий Дамирович', 'Менеджер');
INSERT INTO EMPLOYEE (ID, FIO, POST) VALUES (15, 'Баранов Сергей Романович', 'Программист');


COMMIT WORK;

INSERT INTO INVOICE_TYPE (ID, NAME) VALUES (1, 'Приходная накладная');
INSERT INTO INVOICE_TYPE (ID, NAME) VALUES (2, 'Расходная накладная');


COMMIT WORK;

INSERT INTO INVOICE_HEADER (ID, ID_CONTRACTOR, ID_EMPLOYEE, ID_INVOICE_TYPE, INVOICE_DATE) VALUES (113, 2, 3, 1, '2023-05-30');
INSERT INTO INVOICE_HEADER (ID, ID_CONTRACTOR, ID_EMPLOYEE, ID_INVOICE_TYPE, INVOICE_DATE) VALUES (114, 1, 19, 1, '2023-05-30');
INSERT INTO INVOICE_HEADER (ID, ID_CONTRACTOR, ID_EMPLOYEE, ID_INVOICE_TYPE, INVOICE_DATE) VALUES (115, 1, 4, 1, '2023-05-30');
INSERT INTO INVOICE_HEADER (ID, ID_CONTRACTOR, ID_EMPLOYEE, ID_INVOICE_TYPE, INVOICE_DATE) VALUES (116, 11, 2, 1, '2023-05-30');
INSERT INTO INVOICE_HEADER (ID, ID_CONTRACTOR, ID_EMPLOYEE, ID_INVOICE_TYPE, INVOICE_DATE) VALUES (117, 3, 16, 2, '2023-05-30');
INSERT INTO INVOICE_HEADER (ID, ID_CONTRACTOR, ID_EMPLOYEE, ID_INVOICE_TYPE, INVOICE_DATE) VALUES (118, 3, 4, 1, '2023-05-30');
INSERT INTO INVOICE_HEADER (ID, ID_CONTRACTOR, ID_EMPLOYEE, ID_INVOICE_TYPE, INVOICE_DATE) VALUES (119, 3, 4, 1, '2023-05-30');
INSERT INTO INVOICE_HEADER (ID, ID_CONTRACTOR, ID_EMPLOYEE, ID_INVOICE_TYPE, INVOICE_DATE) VALUES (120, 4, 19, 1, '2023-05-30');
INSERT INTO INVOICE_HEADER (ID, ID_CONTRACTOR, ID_EMPLOYEE, ID_INVOICE_TYPE, INVOICE_DATE) VALUES (121, 5, 4, 2, '2023-05-31');
INSERT INTO INVOICE_HEADER (ID, ID_CONTRACTOR, ID_EMPLOYEE, ID_INVOICE_TYPE, INVOICE_DATE) VALUES (122, 5, 4, 2, '2023-05-31');
INSERT INTO INVOICE_HEADER (ID, ID_CONTRACTOR, ID_EMPLOYEE, ID_INVOICE_TYPE, INVOICE_DATE) VALUES (123, 4, 16, 2, '2023-05-31');
INSERT INTO INVOICE_HEADER (ID, ID_CONTRACTOR, ID_EMPLOYEE, ID_INVOICE_TYPE, INVOICE_DATE) VALUES (124, 3, 19, 2, '2023-05-31');
INSERT INTO INVOICE_HEADER (ID, ID_CONTRACTOR, ID_EMPLOYEE, ID_INVOICE_TYPE, INVOICE_DATE) VALUES (125, 3, 16, 1, '2023-05-31');
INSERT INTO INVOICE_HEADER (ID, ID_CONTRACTOR, ID_EMPLOYEE, ID_INVOICE_TYPE, INVOICE_DATE) VALUES (126, 3, 3, 1, '2023-05-31');
INSERT INTO INVOICE_HEADER (ID, ID_CONTRACTOR, ID_EMPLOYEE, ID_INVOICE_TYPE, INVOICE_DATE) VALUES (127, 2, 2, 2, '2023-05-31');


COMMIT WORK;

INSERT INTO PRODUCT (ID, NAME, TRADEMARK, ID_CATEGORY, PRICE, UNIT, AMOUNT, MIN_STOCK, WANT_STOCK, DESCRIPTION) VALUES (24, 'Картофель Галла', 'Агроцентр Корнеево', 1, 53, 'кг.', 0, 50, 400, 'Ранний картофель');
INSERT INTO PRODUCT (ID, NAME, TRADEMARK, ID_CATEGORY, PRICE, UNIT, AMOUNT, MIN_STOCK, WANT_STOCK, DESCRIPTION) VALUES (25, 'Картофель Синеглазка', 'Агрофирма Металлург', 1, 46.35, 'кг.', 0, 30, 500, 'Отличный вкусный картофель');
INSERT INTO PRODUCT (ID, NAME, TRADEMARK, ID_CATEGORY, PRICE, UNIT, AMOUNT, MIN_STOCK, WANT_STOCK, DESCRIPTION) VALUES (26, 'Мандарины Мароканские', 'Марокко', 2, 68.56, 'кг.', 0, 46, 358, 'Самые лучшие мандарины');
INSERT INTO PRODUCT (ID, NAME, TRADEMARK, ID_CATEGORY, PRICE, UNIT, AMOUNT, MIN_STOCK, WANT_STOCK, DESCRIPTION) VALUES (27, 'Яблоки Антоновка', 'Ильичев', 2, 30, 'кг.', 36, 30, 500, 'Поздние яблоки');
INSERT INTO PRODUCT (ID, NAME, TRADEMARK, ID_CATEGORY, PRICE, UNIT, AMOUNT, MIN_STOCK, WANT_STOCK, DESCRIPTION) VALUES (28, 'Ананас ', 'Кулебакские теплицы', 2, 159, 'шт.', 104, 25, 135, 'Один ананас хорошо, а два - лучше');
INSERT INTO PRODUCT (ID, NAME, TRADEMARK, ID_CATEGORY, PRICE, UNIT, AMOUNT, MIN_STOCK, WANT_STOCK, DESCRIPTION) VALUES (31, 'Груша Конференция', 'Веселый молочник', 2, 104, 'кг.', 0, 56, 325, 'Знаменитый сорт груши');
INSERT INTO PRODUCT (ID, NAME, TRADEMARK, ID_CATEGORY, PRICE, UNIT, AMOUNT, MIN_STOCK, WANT_STOCK, DESCRIPTION) VALUES (32, 'вапва', 'выаппывп', 3, 43, 'кг.', 520, 344, 44, '44');


COMMIT WORK;

INSERT INTO WAREHOUSE (ID, LINE, SHELF, PLACE) VALUES (1, 1, 7, 3);
INSERT INTO WAREHOUSE (ID, LINE, SHELF, PLACE) VALUES (2, 2, 3, 4);
INSERT INTO WAREHOUSE (ID, LINE, SHELF, PLACE) VALUES (3, 3, 4, 5);
INSERT INTO WAREHOUSE (ID, LINE, SHELF, PLACE) VALUES (4, 4, 5, 6);
INSERT INTO WAREHOUSE (ID, LINE, SHELF, PLACE) VALUES (5, 6, 7, 8);
INSERT INTO WAREHOUSE (ID, LINE, SHELF, PLACE) VALUES (6, 9, 10, 11);
INSERT INTO WAREHOUSE (ID, LINE, SHELF, PLACE) VALUES (7, 10, 11, 12);
INSERT INTO WAREHOUSE (ID, LINE, SHELF, PLACE) VALUES (8, 11, 12, 13);
INSERT INTO WAREHOUSE (ID, LINE, SHELF, PLACE) VALUES (9, 14, 15, 16);
INSERT INTO WAREHOUSE (ID, LINE, SHELF, PLACE) VALUES (10, 17, 18, 19);
INSERT INTO WAREHOUSE (ID, LINE, SHELF, PLACE) VALUES (12, 1, 2, 4);
INSERT INTO WAREHOUSE (ID, LINE, SHELF, PLACE) VALUES (13, 4, 54, 6);
INSERT INTO WAREHOUSE (ID, LINE, SHELF, PLACE) VALUES (14, 5, 6, 6);
INSERT INTO WAREHOUSE (ID, LINE, SHELF, PLACE) VALUES (17, 3, 3, 444);
INSERT INTO WAREHOUSE (ID, LINE, SHELF, PLACE) VALUES (16, 4333, 5444, 6555);
INSERT INTO WAREHOUSE (ID, LINE, SHELF, PLACE) VALUES (18, 56, 77, 88);
INSERT INTO WAREHOUSE (ID, LINE, SHELF, PLACE) VALUES (19, 859, 256, 45);
INSERT INTO WAREHOUSE (ID, LINE, SHELF, PLACE) VALUES (20, 777, 777, 777);


COMMIT WORK;

INSERT INTO INVOICE_TABLE_PART (ID, ID_INVOICE_HEADER, ID_PRODUCT, AMOUNT, TOTAL_PRICE, ID_WAREHOUSE) VALUES (7, 113, 24, 4, 212, 7);
INSERT INTO INVOICE_TABLE_PART (ID, ID_INVOICE_HEADER, ID_PRODUCT, AMOUNT, TOTAL_PRICE, ID_WAREHOUSE) VALUES (8, 113, 27, 5, 150, 13);
INSERT INTO INVOICE_TABLE_PART (ID, ID_INVOICE_HEADER, ID_PRODUCT, AMOUNT, TOTAL_PRICE, ID_WAREHOUSE) VALUES (9, 113, 28, 3, 477, 16);
INSERT INTO INVOICE_TABLE_PART (ID, ID_INVOICE_HEADER, ID_PRODUCT, AMOUNT, TOTAL_PRICE, ID_WAREHOUSE) VALUES (10, 114, 28, 8, 1272, 9);
INSERT INTO INVOICE_TABLE_PART (ID, ID_INVOICE_HEADER, ID_PRODUCT, AMOUNT, TOTAL_PRICE, ID_WAREHOUSE) VALUES (11, 114, 28, 7, 1113, 18);
INSERT INTO INVOICE_TABLE_PART (ID, ID_INVOICE_HEADER, ID_PRODUCT, AMOUNT, TOTAL_PRICE, ID_WAREHOUSE) VALUES (12, 115, 24, 66, 3498, 3);
INSERT INTO INVOICE_TABLE_PART (ID, ID_INVOICE_HEADER, ID_PRODUCT, AMOUNT, TOTAL_PRICE, ID_WAREHOUSE) VALUES (13, 115, 28, 66, 10494, 19);
INSERT INTO INVOICE_TABLE_PART (ID, ID_INVOICE_HEADER, ID_PRODUCT, AMOUNT, TOTAL_PRICE, ID_WAREHOUSE) VALUES (14, 115, 25, 77, 3568.95, 5);
INSERT INTO INVOICE_TABLE_PART (ID, ID_INVOICE_HEADER, ID_PRODUCT, AMOUNT, TOTAL_PRICE, ID_WAREHOUSE) VALUES (15, 116, 28, 6, 954, 14);
INSERT INTO INVOICE_TABLE_PART (ID, ID_INVOICE_HEADER, ID_PRODUCT, AMOUNT, TOTAL_PRICE, ID_WAREHOUSE) VALUES (16, 117, 26, 4, 274.24, 6);
INSERT INTO INVOICE_TABLE_PART (ID, ID_INVOICE_HEADER, ID_PRODUCT, AMOUNT, TOTAL_PRICE, ID_WAREHOUSE) VALUES (17, 118, 27, 4, 120, 4);
INSERT INTO INVOICE_TABLE_PART (ID, ID_INVOICE_HEADER, ID_PRODUCT, AMOUNT, TOTAL_PRICE, ID_WAREHOUSE) VALUES (18, 119, 27, 23, 690, 8);
INSERT INTO INVOICE_TABLE_PART (ID, ID_INVOICE_HEADER, ID_PRODUCT, AMOUNT, TOTAL_PRICE, ID_WAREHOUSE) VALUES (19, 120, 27, 3, 90, 17);
INSERT INTO INVOICE_TABLE_PART (ID, ID_INVOICE_HEADER, ID_PRODUCT, AMOUNT, TOTAL_PRICE, ID_WAREHOUSE) VALUES (20, 121, 24, 10, 530, 3);
INSERT INTO INVOICE_TABLE_PART (ID, ID_INVOICE_HEADER, ID_PRODUCT, AMOUNT, TOTAL_PRICE, ID_WAREHOUSE) VALUES (21, 122, 27, 5, 150, 13);
INSERT INTO INVOICE_TABLE_PART (ID, ID_INVOICE_HEADER, ID_PRODUCT, AMOUNT, TOTAL_PRICE, ID_WAREHOUSE) VALUES (22, 123, 24, 5, 265, 3);
INSERT INTO INVOICE_TABLE_PART (ID, ID_INVOICE_HEADER, ID_PRODUCT, AMOUNT, TOTAL_PRICE, ID_WAREHOUSE) VALUES (23, 124, 27, 20, 600, 13);
INSERT INTO INVOICE_TABLE_PART (ID, ID_INVOICE_HEADER, ID_PRODUCT, AMOUNT, TOTAL_PRICE, ID_WAREHOUSE) VALUES (24, 125, 28, 52, 8268, 10);
INSERT INTO INVOICE_TABLE_PART (ID, ID_INVOICE_HEADER, ID_PRODUCT, AMOUNT, TOTAL_PRICE, ID_WAREHOUSE) VALUES (25, 125, 28, 52, 8268, 20);
INSERT INTO INVOICE_TABLE_PART (ID, ID_INVOICE_HEADER, ID_PRODUCT, AMOUNT, TOTAL_PRICE, ID_WAREHOUSE) VALUES (26, 126, 27, 15, 450, 12);
INSERT INTO INVOICE_TABLE_PART (ID, ID_INVOICE_HEADER, ID_PRODUCT, AMOUNT, TOTAL_PRICE, ID_WAREHOUSE) VALUES (27, 126, 32, 525, 22575, 2);
INSERT INTO INVOICE_TABLE_PART (ID, ID_INVOICE_HEADER, ID_PRODUCT, AMOUNT, TOTAL_PRICE, ID_WAREHOUSE) VALUES (28, 127, 32, 5, 215, 2);


COMMIT WORK;



/******************************************************************************/
/***                        Autoincrement generators                        ***/
/******************************************************************************/

ALTER TABLE CATEGORY ALTER ID RESTART WITH 3;
ALTER TABLE CONTRACTOR ALTER ID RESTART WITH 11;
ALTER TABLE EMPLOYEE ALTER ID RESTART WITH 19;
ALTER TABLE INVOICE_TABLE_PART ALTER ID RESTART WITH 28;
ALTER TABLE INVOICE_TYPE ALTER ID RESTART WITH 2;
ALTER TABLE PRODUCT ALTER ID RESTART WITH 32;
ALTER TABLE WAREHOUSE ALTER ID RESTART WITH 20;




/******************************************************************************/
/***                              Primary keys                              ***/
/******************************************************************************/

ALTER TABLE CATEGORY ADD CONSTRAINT PK_CATEGORY PRIMARY KEY (ID);
ALTER TABLE CONTRACTOR ADD CONSTRAINT PK_CONTRACTOR PRIMARY KEY (ID);
ALTER TABLE EMPLOYEE ADD CONSTRAINT PK_EMPLOYEE PRIMARY KEY (ID);
ALTER TABLE INVOICE_HEADER ADD CONSTRAINT PK_INVOICE_HEADER PRIMARY KEY (ID);
ALTER TABLE INVOICE_TABLE_PART ADD CONSTRAINT PK_INVOICE_TABLE_PART PRIMARY KEY (ID);
ALTER TABLE INVOICE_TYPE ADD CONSTRAINT PK_INVOICE_TYPE PRIMARY KEY (ID);
ALTER TABLE PRODUCT ADD CONSTRAINT PK_PRODUCT PRIMARY KEY (ID);
ALTER TABLE WAREHOUSE ADD CONSTRAINT PK_WAREHOUSE PRIMARY KEY (ID);


/******************************************************************************/
/***                              Foreign keys                              ***/
/******************************************************************************/

ALTER TABLE INVOICE_HEADER ADD CONSTRAINT FK_INVOICE_HEADER_1 FOREIGN KEY (ID_CONTRACTOR) REFERENCES CONTRACTOR (ID);
ALTER TABLE INVOICE_HEADER ADD CONSTRAINT FK_INVOICE_HEADER_2 FOREIGN KEY (ID_EMPLOYEE) REFERENCES EMPLOYEE (ID);
ALTER TABLE INVOICE_HEADER ADD CONSTRAINT FK_INVOICE_HEADER_3 FOREIGN KEY (ID_INVOICE_TYPE) REFERENCES INVOICE_TYPE (ID);
ALTER TABLE INVOICE_TABLE_PART ADD CONSTRAINT FK_INVOICE_TABLE_PART_1 FOREIGN KEY (ID_INVOICE_HEADER) REFERENCES INVOICE_HEADER (ID);
ALTER TABLE INVOICE_TABLE_PART ADD CONSTRAINT FK_INVOICE_TABLE_PART_2 FOREIGN KEY (ID_PRODUCT) REFERENCES PRODUCT (ID);
ALTER TABLE INVOICE_TABLE_PART ADD CONSTRAINT FK_INVOICE_TABLE_PART_3 FOREIGN KEY (ID_WAREHOUSE) REFERENCES WAREHOUSE (ID);
ALTER TABLE PRODUCT ADD CONSTRAINT FK_PRODUCT_2 FOREIGN KEY (ID_CATEGORY) REFERENCES CATEGORY (ID);


/******************************************************************************/
/***                                Triggers                                ***/
/******************************************************************************/



SET TERM ^ ;



/******************************************************************************/
/***                          Triggers for tables                           ***/
/******************************************************************************/



/* Trigger: INVOICE_HEADER_BI */
CREATE TRIGGER INVOICE_HEADER_BI FOR INVOICE_HEADER
ACTIVE BEFORE INSERT POSITION 0
as
begin
  if (new.id is null) then
    new.id = gen_id(gen_invoice_header_id,1);
end
^
SET TERM ; ^

