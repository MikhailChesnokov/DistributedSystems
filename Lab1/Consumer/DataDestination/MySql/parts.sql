create table client
(
  Id          int auto_increment
    primary key,
  Email       longtext null,
  Name        longtext null,
  PhoneNumber longtext null
);

create table `order`
(
  Id       int auto_increment
    primary key,
  ClientId int         null,
  Date     datetime(6) not null,
  Number   int         not null,
  constraint FK_Order_Client_ClientId
    foreign key (ClientId) references client (id)
);

create index IX_Order_ClientId
  on `order` (ClientId);

create table part
(
  Id       int auto_increment
    primary key,
  Name     longtext null,
  Presence bit      not null,
  Price    double   not null
);

create table orderpart
(
  Id      int auto_increment
    primary key,
  OrderId int null,
  PartId  int null,
  constraint FK_OrderPart_Order_OrderId
    foreign key (OrderId) references `order` (id),
  constraint FK_OrderPart_Part_PartId
    foreign key (PartId) references part (id)
);

create index IX_OrderPart_OrderId
  on orderpart (OrderId);

create index IX_OrderPart_PartId
  on orderpart (PartId);

create table shop
(
  Id      int auto_increment
    primary key,
  Address longtext null,
  City    longtext null
);

create table partshop
(
  Id     int auto_increment
    primary key,
  PartId int null,
  ShopId int null,
  constraint FK_PartShop_Part_PartId
    foreign key (PartId) references part (id),
  constraint FK_PartShop_Shop_ShopId
    foreign key (ShopId) references shop (id)
);

create index IX_PartShop_PartId
  on partshop (PartId);

create index IX_PartShop_ShopId
  on partshop (ShopId);

