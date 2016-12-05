use master
go

if exists(select * from sys.databases where name='CompanyDB')
	drop database CompanyDB
go

create database CompanyDB
go

use CompanyDB
go

--部门表
create table Department
(
    D_ID int primary key identity(1,1),
	D_Name varchar(20) not null,
)

--员工表
create table Userinfo
(
    U_ID varchar(50) primary key ,
	Name varchar(20)   null,  --姓名
	Pwd varchar(64)  null,--密码
	Sex char(2) check(sex='男' or sex ='女'),--性别
	Age int  null,
	Tel varchar(12)  null,
	Images varchar(200)  null,
	D_ID int references Department(D_ID),
	IsManage char(1) check(IsManage='0' or IsManage='1' or IsManage='2') not null        --0是普通用户，1是部门管理员，2是大boss
)

-------------------------------------公告-------------------------------------

--公告表
create table Announcement
(
    A_ID int primary key identity(1,1) ,
	U_ID varchar(50) references Userinfo (U_ID),
	A_Time datetime default(getdate()),
    A_Image varchar(100) null,
	A_Title varchar(50) not null,
	A_Content varchar(5000)  not null
)
--公告已未读信息
create table IsRead 
(
    I_ID int primary key identity(1,1),
	U_ID varchar(50) references Userinfo(U_ID),
	A_ID int references Announcement(A_ID)
)
select * from Announcement
select * from IsRead
-------------------------------------签到------------------------------------

--签到表
create table Registration
(
    R_ID int primary key identity(1,1),--签到编号
	U_ID varchar(50) references Userinfo(U_ID),
	R_Site varchar(200) not null,
	R_Time datetime default(getdate()) not null,
	R_Remark varchar(200)
)
--select * from Userinfo where U_ID='0443103313939774';
--select s2.Name,(select count(*) from Registration where datediff(month,R_Time,getdate())=0) 当天签到次数  from Registration s1 inner join Userinfo s2 on  s1.U_ID=s2.U_ID where s1.U_ID='0443103313992' and datediff(month,R_Time,getdate())=0
--select top 1 s2.Name,(select count(*) from Registration where datediff(month,R_Time,getdate())=0) R_ID  from Registration s1 inner join Userinfo s2 on  s1.U_ID=s2.U_ID where s1.U_ID='0443103313992' and datediff(month,R_Time,getdate())=0
--select * from Registration
--------------------------------------审批功能-------------------------------------



--请假表
create table Vacateinfo
(
    V_ID int primary key identity(1,1), --请假编号
	U_ID varchar(50) references Userinfo(U_ID),
	V_Type varchar(50) not null,
	V_StartTime datetime not null, --开始时间
	V_EndTime datetime not null,--结束时间
	V_Days int not null ,--请假天数
	V_Remark varchar(200) not null,--请假事由
    A_U_ID varchar(50) references Userinfo(U_ID),	--审批人
	IsAdmission char(1) check(IsAdmission=1 or IsAdmission=2 or IsAdmission=3) null,--是否通过审批
	IsApprove bit not null default(0)--是否审批
)
--出差表
create table Travelinfo
(
    T_ID int primary key identity(1,1),--出差编号
	U_ID varchar(50) references Userinfo(U_ID),
	T_Site varchar(50) not null,--出差地点
	T_StartTime datetime not null,
	T_EndTime datetime not null,
	T_Days int not null,--出差天数
	T_Remark varchar(200) not null,--出差事由 
	A_U_ID varchar(50) references Userinfo(U_ID),--审批人
	IsAdmission char(1) check(IsAdmission=1 or IsAdmission=2 or IsAdmission=3) null,--是否通过审批
	IsApprove bit not null default(0)--是否审批
)


select * from  Vacateinfo
--报销表
create table Reimburse
(
    R_ID int primary key identity(1,1),
	U_ID varchar(50) references Userinfo(U_ID),
	R_Money decimal not null,
	R_Type varchar(50) not null, --报销类型 如：采购经费、活动经费...
	R_Remark varchar(200) ,
	A_U_ID varchar(50) references Userinfo(U_ID),	--审批人
    IsAdmission char(1) check(IsAdmission=1 or IsAdmission=2 or IsAdmission=3) null,--是否通过审批
	IsApprove bit not null default(0)--是否审批
)
----------------------------------预约会议室--------------------------------
--会议室表
create table MeetingRoom
(
     M_ID int primary key identity(1,1),
	 M_Site varchar(200) not null, --位置
	 M_Floor  varchar(100) not null , --楼层
	 M_Name varchar(20) not null,--会议室名称
	 M_PeopleNum int not null,--可容纳人数
	 M_IsWIFI bit default(1),--是否支持WIFI
	 M_Projection bit, --是否有投影
)

--预约会议室
create table Subscribe
(
     S_ID int primary key identity(1,1),
	 M_ID int references MeetingRoom(M_ID),
	 M_Date datetime not null,
	 M_StartTime varchar(10) not null,
	 M_EndTime varchar(10) not null,
	 M_Title varchar(50) not null,--会议主题
	 A_U_ID varchar(50) references Userinfo(U_ID),	--审批人
)
select * from Registration
select * from Userinfo s1 inner join Subscribe s2 on s1.U_ID=s2.A_U_ID
inner join MeetingRoom s3 on s2.M_ID=s3.M_ID



--公告信息测试数据及存储过程
--查询未读公告信息
if exists(select * from  sys.objects where name='proc_SelNoread')
drop proc proc_SelNoread
go
create proc proc_SelNoread
@id varchar(50)
as
select * from Announcement s1 inner join Userinfo s2 on s1.U_ID=s2.U_ID  where s1.A_ID not in (select A_id from IsRead where U_ID =@id) order by s1.A_Time desc
go

--查询已读公告信息
if exists(select * from  sys.objects where name='proc_Selread')
drop proc proc_Selread
go
create proc proc_Selread
@id varchar(50)
as
select *  from Announcement s1 inner join IsRead s2 on s1.a_ID = s2.a_ID inner join Userinfo s3 on s1.U_ID=s3.U_ID  where s2.U_ID=@id order by s1.A_ID asc
go
--条件查询会议室
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
----------------判断时间是否冲突，如果冲突就用下面添加存储过程-------------------------------------
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
----------------时间查询已预约会议室-------------------------------------
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


--------------------------------------------------未审批-------------------------------------------------
----------------未审批请假-------------------
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
------------------------未审批出差-------------------
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

--------------------未审批报销-------------------
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

--------------------------------------------------未审批-------------------------------------------------


--------------------------------------------------发起的审批-------------------------------------------------

------------------------查询我发起的请假----------------------
if exists(select * from sys.objects where name='proc_MyVacate')
drop proc proc_MyVacate
go
create proc proc_MyVacate
@U_ID varchar(50)
as
select * from Vacateinfo where U_ID=@U_ID
go

------------------------查询我发起的出差----------------------
if exists(select * from sys.objects where name='proc_MyTravel')
drop proc proc_MyTravel
go
create proc proc_MyTravel
@U_ID varchar(50)
as
select * from Travelinfo where U_ID=@U_ID
go

------------------------查询我发起的报销----------------------
if exists(select * from sys.objects where name='proc_MyReimburse')
drop proc proc_MyReimburse
go
create proc proc_MyReimburse
@U_ID varchar(50)
as
select * from Reimburse where U_ID=@U_ID
go

--------------------------------------------------发起的审批-------------------------------------------------

--------------------------------------------------已审批-------------------------------------------------

-------------------------------已审批请假------------------------------
if exists(select * from sys.objects where name='proc_AlreadyVacate')
drop proc proc_IsVacate
go
create proc proc_AlreadyVacate
@U_ID varchar(50),
@IsApprove bit
as
select * from Vacateinfo where U_ID=@U_ID and IsApprove=@IsApprove
go

------------------------已审批出差-------------------
if exists(select * from sys.objects where name='proc_AlreadyTravel')
drop proc proc_IsTravel
go
create proc proc_AlreadyTravel
@U_ID varchar(50),
@IsApprove bit
as
select * from Travelinfo where U_ID=@U_ID and IsApprove=@IsApprove 
go

----------------------已审批报销-------------------
if exists(select * from sys.objects where name='proc_AlreadyReimburse')
drop proc proc_IsReimburse
go
create proc proc_AlreadyReimburse
@U_ID varchar(50),
@IsApprove bit
as
select * from Reimburse where U_ID=@U_ID and IsApprove=@IsApprove 
go

---------------------------------------按钮确认-------------------------------------------
--------------------------管理员确认请假审批------------------------
if exists(select * from sys.objects where name='proc_IsPassVacate')
drop proc proc_IsPassVacate
go
create proc proc_IsPassVacate
@V_ID int,
@IsAdmission char(1)
as
update Vacateinfo set IsApprove=1,IsAdmission=@IsAdmission where V_ID=@V_ID
go
--------------------------管理员确认出差审批------------------------
if exists(select * from sys.objects where name='proc_IsPassTravel')
drop proc proc_IsPassTravel
go
create proc proc_IsPassTravel
@T_ID varchar(50),
@IsAdmission char(1)
as
update Travelinfo set IsApprove=1,IsAdmission=@IsAdmission  where T_ID=@T_ID
go
--------------------------管理员确认报销审批------------------------

if exists(select * from sys.objects where name='proc_IsPassReimburse')
drop proc proc_IsPassReimburse
go
create proc proc_IsPassReimburse
@R_ID varchar(50),
@IsAdmission char(1)
as
update Reimburse set IsApprove=1,IsAdmission=@IsAdmission  where R_ID=@R_ID
go

-------------------------------测试数据-----------------------------
insert into Department values('hr')
insert into Department values('公关部')
insert into Department values('后勤部')
insert into Department values('销售部')
insert into Department values('市场部')
insert into Department values('公司总部')
insert into Userinfo values('0443103313939774','张三',123,'男',20,'15827270280','',6,2)
insert into Userinfo values('046168114237552223','李四',123,'男',20,'15827270280','',6,2)
insert into Userinfo values('04431320566998','王五',123,'男',20,'15827270280','',6,2)
insert into Userinfo values('04362343542545','Mike',123,'男',20,'15827270280','',6,2)
insert into Userinfo values('064126203862','Lily',123,'男',20,'15827270280','',6,2)
insert into Userinfo values('04430233451247','Eric',123,'男',20,'15827270280','',6,2)
insert into Userinfo values('081957439888','Tom',123,'男',20,'15827270280','',6,2)
insert into Userinfo values('manager3231','WangHailin',123,'男',21,'15827270280','',6,2)
insert into Announcement values('04431320566998',default,'','我发誓','好消息！我发誓，我是大傻逼！');
insert into Announcement values('04362343542545',default,'','坚持','孩子们最后阶段了，千万不要放弃啊！');
insert into Announcement values('064126203862',default,'','加油','小伙子们，我相信你们可以的，一定要加油啊！');
insert into Announcement values('04430233451247',default,'','不要放弃','兄弟们，加油啊，最后阶段了千万不要选择放弃啊，不然我们前面就白干了！');
insert into Announcement values('081957439888',default,'','哈哈哈哈哈','什么玩意哟，我这美工做得还是很辛苦的好吧，你们可不要让我白做！');
insert into MeetingRoom values('图书馆204','1','机房',60,default,0);
insert into MeetingRoom values('图书馆104','1','机房',60,0,1);
insert into MeetingRoom values('4号教学楼','1','公共教室',120,default,0);
insert into Subscribe values(1,'2016-6-3','8:30','10:00','开会了sdfsdfs','04431320566998')
insert into Subscribe values(2,'2016-5-15','10:30','12:00','放假会议','04430233451247')
insert into Subscribe values(3,'2016-6-1','14:00','15:30','给你永久的假期','081957439888')
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

