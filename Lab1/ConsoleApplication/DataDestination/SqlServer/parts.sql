create table Client
(
  Id          int identity
    constraint PK_Client
      primary key,
  Email       nvarchar(max),
  Name        nvarchar(max),
  PhoneNumber nvarchar(max)
)
go

create table [Order]
(
  Id       int identity
    constraint PK_Order
      primary key,
  ClientId int
    constraint FK_Order_Client_ClientId
      references Client,
  Date     datetime2 not null,
  Number   int       not null
)
go

create index IX_Order_ClientId
  on [Order] (ClientId)
go

create table Part
(
  Id       int identity
    constraint PK_Part
      primary key,
  Name     nvarchar(max),
  Presence bit   not null,
  Price    float not null
)
go

create table OrderPart
(
  Id      int identity
    constraint PK_OrderPart
      primary key,
  OrderId int
    constraint FK_OrderPart_Order_OrderId
      references [Order],
  PartId  int
    constraint FK_OrderPart_Part_PartId
      references Part
)
go

create index IX_OrderPart_OrderId
  on OrderPart (OrderId)
go

create index IX_OrderPart_PartId
  on OrderPart (PartId)
go

create table Shop
(
  Id      int identity
    constraint PK_Shop
      primary key,
  Address nvarchar(max),
  City    nvarchar(max)
)
go

create table PartShop
(
  Id     int identity
    constraint PK_PartShop
      primary key,
  PartId int
    constraint FK_PartShop_Part_PartId
      references Part,
  ShopId int
    constraint FK_PartShop_Shop_ShopId
      references Shop
)
go

create index IX_PartShop_PartId
  on PartShop (PartId)
go

create index IX_PartShop_ShopId
  on PartShop (ShopId)
go

