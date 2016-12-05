use master
go

if exists(select * from sys.databases where name='CompanyDB')
	drop database CompanyDB
go

create database CompanyDB
go

use CompanyDB
go

--���ű�
create table Department
(
    D_ID int primary key identity(1,1),
	D_Name varchar(20) not null,
)

--Ա����
create table Userinfo
(
    U_ID varchar(50) primary key ,
	Name varchar(20)   null,  --����
	Pwd varchar(64)  null,--����
	Sex char(2) check(sex='��' or sex ='Ů'),--�Ա�
	Age int  null,
	Tel varchar(12)  null,
	Images varchar(200)  null,
	D_ID int references Department(D_ID),
	IsManage char(1) check(IsManage='0' or IsManage='1' or IsManage='2') not null        --0����ͨ�û���1�ǲ��Ź���Ա��2�Ǵ�boss
)

-------------------------------------����-------------------------------------

--�����
create table Announcement
(
    A_ID int primary key identity(1,1) ,
	U_ID varchar(50) references Userinfo (U_ID),
	A_Time datetime default(getdate()),
    A_Image varchar(100) null,
	A_Title varchar(50) not null,
	A_Content varchar(5000)  not null
)
--������δ����Ϣ
create table IsRead 
(
    I_ID int primary key identity(1,1),
	U_ID varchar(50) references Userinfo(U_ID),
	A_ID int references Announcement(A_ID)
)
select * from Announcement
select * from IsRead
-------------------------------------ǩ��------------------------------------

--ǩ����
create table Registration
(
    R_ID int primary key identity(1,1),--ǩ�����
	U_ID varchar(50) references Userinfo(U_ID),
	R_Site varchar(200) not null,
	R_Time datetime default(getdate()) not null,
	R_Remark varchar(200)
)
--select * from Userinfo where U_ID='0443103313939774';
--select s2.Name,(select count(*) from Registration where datediff(month,R_Time,getdate())=0) ����ǩ������  from Registration s1 inner join Userinfo s2 on  s1.U_ID=s2.U_ID where s1.U_ID='0443103313992' and datediff(month,R_Time,getdate())=0
--select top 1 s2.Name,(select count(*) from Registration where datediff(month,R_Time,getdate())=0) R_ID  from Registration s1 inner join Userinfo s2 on  s1.U_ID=s2.U_ID where s1.U_ID='0443103313992' and datediff(month,R_Time,getdate())=0
--select * from Registration
--------------------------------------��������-------------------------------------



--��ٱ�
create table Vacateinfo
(
    V_ID int primary key identity(1,1), --��ٱ��
	U_ID varchar(50) references Userinfo(U_ID),
	V_Type varchar(50) not null,
	V_StartTime datetime not null, --��ʼʱ��
	V_EndTime datetime not null,--����ʱ��
	V_Days int not null ,--�������
	V_Remark varchar(200) not null,--�������
    A_U_ID varchar(50) references Userinfo(U_ID),	--������
	IsAdmission char(1) check(IsAdmission=1 or IsAdmission=2 or IsAdmission=3) null,--�Ƿ�ͨ������
	IsApprove bit not null default(0)--�Ƿ�����
)
--�����
create table Travelinfo
(
    T_ID int primary key identity(1,1),--������
	U_ID varchar(50) references Userinfo(U_ID),
	T_Site varchar(50) not null,--����ص�
	T_StartTime datetime not null,
	T_EndTime datetime not null,
	T_Days int not null,--��������
	T_Remark varchar(200) not null,--�������� 
	A_U_ID varchar(50) references Userinfo(U_ID),--������
	IsAdmission char(1) check(IsAdmission=1 or IsAdmission=2 or IsAdmission=3) null,--�Ƿ�ͨ������
	IsApprove bit not null default(0)--�Ƿ�����
)


select * from  Vacateinfo
--������
create table Reimburse
(
    R_ID int primary key identity(1,1),
	U_ID varchar(50) references Userinfo(U_ID),
	R_Money decimal not null,
	R_Type varchar(50) not null, --�������� �磺�ɹ����ѡ������...
	R_Remark varchar(200) ,
	A_U_ID varchar(50) references Userinfo(U_ID),	--������
    IsAdmission char(1) check(IsAdmission=1 or IsAdmission=2 or IsAdmission=3) null,--�Ƿ�ͨ������
	IsApprove bit not null default(0)--�Ƿ�����
)
----------------------------------ԤԼ������--------------------------------
--�����ұ�
create table MeetingRoom
(
     M_ID int primary key identity(1,1),
	 M_Site varchar(200) not null, --λ��
	 M_Floor  varchar(100) not null , --¥��
	 M_Name varchar(20) not null,--����������
	 M_PeopleNum int not null,--����������
	 M_IsWIFI bit default(1),--�Ƿ�֧��WIFI
	 M_Projection bit, --�Ƿ���ͶӰ
)

--ԤԼ������
create table Subscribe
(
     S_ID int primary key identity(1,1),
	 M_ID int references MeetingRoom(M_ID),
	 M_Date datetime not null,
	 M_StartTime varchar(10) not null,
	 M_EndTime varchar(10) not null,
	 M_Title varchar(50) not null,--��������
	 A_U_ID varchar(50) references Userinfo(U_ID),	--������
)
select * from Registration
select * from Userinfo s1 inner join Subscribe s2 on s1.U_ID=s2.A_U_ID
inner join MeetingRoom s3 on s2.M_ID=s3.M_ID



--������Ϣ�������ݼ��洢����
--��ѯδ��������Ϣ
if exists(select * from  sys.objects where name='proc_SelNoread')
drop proc proc_SelNoread
go
create proc proc_SelNoread
@id varchar(50)
as
select * from Announcement s1 inner join Userinfo s2 on s1.U_ID=s2.U_ID  where s1.A_ID not in (select A_id from IsRead where U_ID =@id) order by s1.A_Time desc
go

--��ѯ�Ѷ�������Ϣ
if exists(select * from  sys.objects where name='proc_Selread')
drop proc proc_Selread
go
create proc proc_Selread
@id varchar(50)
as
select *  from Announcement s1 inner join IsRead s2 on s1.a_ID = s2.a_ID inner join Userinfo s3 on s1.U_ID=s3.U_ID  where s2.U_ID=@id order by s1.A_ID asc
go
--������ѯ������
if exists(select * from sys.objects where name='proc_Sel')
drop proc proc_Sel
go

create proc proc_Sel
@Iswifi bit,
@M_Projection bit
as
if @Iswifi=1 and @M_Projection=0
begin
  select * from MeetingRoom where M_IsWIFI=@Iswifi
end
else if @Iswifi=0 and @M_Projection=1
begin
  select * from MeetingRoom where M_Projection=@M_Projection
end
else if @Iswifi=1 and @M_Projection=1
begin
  select * from MeetingRoom where M_Projection=@M_Projection and M_IsWIFI=@Iswifi
end
else if @Iswifi=0 and @M_Projection=0
begin
 select * from MeetingRoom where M_Projection=@M_Projection and M_IsWIFI=@Iswifi
end
go
----------------�ж�ʱ���Ƿ��ͻ�������ͻ����������Ӵ洢����-------------------------------------
--if exists(select * from sys.objects where name='proc_selRoom')
--drop proc proc_selRoom
--go
--create proc proc_selRoom
--@starttime datetime,
--@endtime datetime,
--@mid int
--as  
--select * from MeetingRoom s1 inner join  Subscribe s2 on   s1.M_ID=s2.M_ID
-- where ((@starttime between M_StartTime and M_EndTime ) or(@endtime between M_StartTime and M_EndTime) or (M_StartTime > @starttime and  @endtime>M_EndTime )) and s1.M_ID = @mid
--go
----------------ʱ���ѯ��ԤԼ������-------------------------------------
--if exists(select * from sys.objects where name='proc_selAlreadyRoom')
--drop proc proc_selAlreadyRoom
--go

--create proc proc_selAlreadyRoom
--@starttime datetime,
--@endtime datetime
--as  
--select  s1.M_ID,S_ID,M_StartTime,M_EndTime,M_Title,A_U_ID
-- from MeetingRoom s1 inner join  Subscribe s2 on   s1.M_ID=s2.M_ID
-- where (@starttime between M_StartTime and M_EndTime ) or(@endtime between M_StartTime and M_EndTime) or (M_StartTime > @starttime and  @endtime>M_EndTime ) 
--go

if exists(select * from sys.objects where name='proc_insertRoom')
drop proc proc_insertRoom
go

create proc proc_insertRoom
@M_ID int,
@M_Date datetime,
@S_StartTime varchar(10),
@S_EndTime varchar(10),
@S_Title varchar(50),
@S_U_ID int
as 
insert into Subscribe values(@M_ID,@M_Date,@S_StartTime,@S_EndTime,@S_Title,@S_U_ID)
go


--------------------------------------------------δ����-------------------------------------------------
----------------δ�������-------------------
if exists(select * from sys.objects where name='proc_IsVacate')
drop proc proc_IsVacate
go
create proc proc_IsVacate
@U_ID varchar(50),
@IsApprove bit
as
if @IsApprove =0
begin
select * from Vacateinfo where  IsApprove=@IsApprove and IsAdmission=3 and A_U_ID=@U_ID
end
else
begin
select * from Vacateinfo where  IsApprove=@IsApprove and IsAdmission=3 and A_U_ID=@U_ID
end
go
--exec proc_IsVacate '',0
------------------------δ��������-------------------
if exists(select * from sys.objects where name='proc_IsTravel')
drop proc proc_IsTravel
go
create proc proc_IsTravel
@U_ID varchar(50),
@IsApprove bit
as
if @IsApprove =0
begin
select * from Travelinfo where  IsApprove=@IsApprove and IsAdmission=3 and A_U_ID=@U_ID
end
else
begin
select * from Travelinfo where  IsApprove=@IsApprove and IsAdmission=3 and A_U_ID=@U_ID
end
go

--------------------δ��������-------------------
if exists(select * from sys.objects where name='proc_IsReimburse')
drop proc proc_IsReimburse
go
create proc proc_IsReimburse
@U_ID varchar(50),
@IsApprove bit
as
if @IsApprove =0
begin
select * from Reimburse where  IsApprove=@IsApprove and IsAdmission=3 and A_U_ID=@U_ID
end
else
begin
select * from Reimburse where  IsApprove=@IsApprove and IsAdmission=3 and A_U_ID=@U_ID
end
go

--------------------------------------------------δ����-------------------------------------------------


--------------------------------------------------���������-------------------------------------------------

------------------------��ѯ�ҷ�������----------------------
if exists(select * from sys.objects where name='proc_MyVacate')
drop proc proc_MyVacate
go
create proc proc_MyVacate
@U_ID varchar(50)
as
select * from Vacateinfo where U_ID=@U_ID
go

------------------------��ѯ�ҷ���ĳ���----------------------
if exists(select * from sys.objects where name='proc_MyTravel')
drop proc proc_MyTravel
go
create proc proc_MyTravel
@U_ID varchar(50)
as
select * from Travelinfo where U_ID=@U_ID
go

------------------------��ѯ�ҷ���ı���----------------------
if exists(select * from sys.objects where name='proc_MyReimburse')
drop proc proc_MyReimburse
go
create proc proc_MyReimburse
@U_ID varchar(50)
as
select * from Reimburse where U_ID=@U_ID
go

--------------------------------------------------���������-------------------------------------------------

--------------------------------------------------������-------------------------------------------------

-------------------------------���������------------------------------
if exists(select * from sys.objects where name='proc_AlreadyVacate')
drop proc proc_IsVacate
go
create proc proc_AlreadyVacate
@U_ID varchar(50),
@IsApprove bit
as
select * from Vacateinfo where U_ID=@U_ID and IsApprove=@IsApprove
go

------------------------����������-------------------
if exists(select * from sys.objects where name='proc_AlreadyTravel')
drop proc proc_IsTravel
go
create proc proc_AlreadyTravel
@U_ID varchar(50),
@IsApprove bit
as
select * from Travelinfo where U_ID=@U_ID and IsApprove=@IsApprove 
go

----------------------����������-------------------
if exists(select * from sys.objects where name='proc_AlreadyReimburse')
drop proc proc_IsReimburse
go
create proc proc_AlreadyReimburse
@U_ID varchar(50),
@IsApprove bit
as
select * from Reimburse where U_ID=@U_ID and IsApprove=@IsApprove 
go

---------------------------------------��ťȷ��-------------------------------------------
--------------------------����Աȷ���������------------------------
if exists(select * from sys.objects where name='proc_IsPassVacate')
drop proc proc_IsPassVacate
go
create proc proc_IsPassVacate
@V_ID int,
@IsAdmission char(1)
as
update Vacateinfo set IsApprove=1,IsAdmission=@IsAdmission where V_ID=@V_ID
go
--------------------------����Աȷ�ϳ�������------------------------
if exists(select * from sys.objects where name='proc_IsPassTravel')
drop proc proc_IsPassTravel
go
create proc proc_IsPassTravel
@T_ID varchar(50),
@IsAdmission char(1)
as
update Travelinfo set IsApprove=1,IsAdmission=@IsAdmission  where T_ID=@T_ID
go
--------------------------����Աȷ�ϱ�������------------------------

if exists(select * from sys.objects where name='proc_IsPassReimburse')
drop proc proc_IsPassReimburse
go
create proc proc_IsPassReimburse
@R_ID varchar(50),
@IsAdmission char(1)
as
update Reimburse set IsApprove=1,IsAdmission=@IsAdmission  where R_ID=@R_ID
go

-------------------------------��������-----------------------------
insert into Department values('hr')
insert into Department values('���ز�')
insert into Department values('���ڲ�')
insert into Department values('���۲�')
insert into Department values('�г���')
insert into Department values('��˾�ܲ�')
insert into Userinfo values('0443103313939774','����',123,'��',20,'15827270280','',6,2)
insert into Userinfo values('046168114237552223','����',123,'��',20,'15827270280','',6,2)
insert into Userinfo values('04431320566998','����',123,'��',20,'15827270280','',6,2)
insert into Userinfo values('04362343542545','Mike',123,'��',20,'15827270280','',6,2)
insert into Userinfo values('064126203862','Lily',123,'��',20,'15827270280','',6,2)
insert into Userinfo values('04430233451247','Eric',123,'��',20,'15827270280','',6,2)
insert into Userinfo values('081957439888','Tom',123,'��',20,'15827270280','',6,2)
insert into Userinfo values('manager3231','WangHailin',123,'��',21,'15827270280','',6,2)
insert into Announcement values('04431320566998',default,'','�ҷ���','����Ϣ���ҷ��ģ����Ǵ�ɵ�ƣ�');
insert into Announcement values('04362343542545',default,'','���','���������׶��ˣ�ǧ��Ҫ��������');
insert into Announcement values('064126203862',default,'','����','С�����ǣ����������ǿ��Եģ�һ��Ҫ���Ͱ���');
insert into Announcement values('04430233451247',default,'','��Ҫ����','�ֵ��ǣ����Ͱ������׶���ǧ��Ҫѡ�����������Ȼ����ǰ��Ͱ׸��ˣ�');
insert into Announcement values('081957439888',default,'','����������','ʲô����Ӵ�������������û��Ǻ�����ĺðɣ����ǿɲ�Ҫ���Ұ�����');
insert into MeetingRoom values('ͼ���204','1','����',60,default,0);
insert into MeetingRoom values('ͼ���104','1','����',60,0,1);
insert into MeetingRoom values('4�Ž�ѧ¥','1','��������',120,default,0);
insert into Subscribe values(1,'2016-6-3','8:30','10:00','������sdfsdfs','04431320566998')
insert into Subscribe values(2,'2016-5-15','10:30','12:00','�żٻ���','04430233451247')
insert into Subscribe values(3,'2016-6-1','14:00','15:30','�������õļ���','081957439888')
-----------------------------------------------

select * from Userinfo
select * from Announcement
select * from IsRead
select * from Subscribe
select * from MeetingRoom s1 inner join  Subscribe s2 on   s1.M_ID=s2.M_ID
select * from Reimburse
select * from Vacateinfo
select * from Travelinfo

select * from Userinfo s1 inner join Subscribe s2 on s1.U_ID=s2.A_U_ID inner join MeetingRoom s3 on s2.M_ID=s3.M_ID order by s2.M_Date  desc,s2.M_StartTime asc

