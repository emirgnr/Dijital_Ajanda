create database DB_AJANDA

use DB_AJANDA
go

create table kullanicilar(

	id int primary key identity (1,1) not null,
	kullanici_adi nvarchar(50),
	sifre nvarchar(50)
)

insert into kullanicilar values ('emirguner','12345')
insert into kullanicilar values ('aleynaozlemyildirim','1234')
insert into kullanicilar values ('berkantahmadi','123')
insert into kullanicilar values ('adnankursatteke','123')

select * from kullanicilar


create table gorsellerim(

	gorsel_id int primary key identity (1,1) not null,
	kullanici_adi nvarchar(50),
	gorsel_adi nvarchar(50),
	gorsel_Data varbinary(max)

)

select * from gorsellerim

create table yapilacaklarim(

	yapilacak_id int primary key identity (1,1) not null,
	kullanici_adi nvarchar(50),
	yapilacak_metni varchar(500)

)

select * from yapilacaklarim

create table tamamlananlarim(

	tamamlanan_id int primary key identity (1,1) not null,
	kullanici_adi nvarchar(50),
	tamamlanan_metni varchar(500)

)

select * from tamamlananlarim


create table notlarim(

	not_id int primary key identity (1,1) not null,
	kullanici_adi nvarchar(50),
	not_tarih date,
	not_baslik varchar(50),
	not_metni varchar(max),
	not_gorselAdi nvarchar(255),
	not_gorselData varbinary(max),
	not_sesAdi nvarchar(255),
	not_sesData varbinary(max)

)

select * from notlarim

create table seslerim(

	ses_id int primary key identity (1,1) not null,
	kullanici_adi nvarchar(50),
	ses_adi nvarchar(50),
	ses_Data varbinary(max)

)

select * from seslerim