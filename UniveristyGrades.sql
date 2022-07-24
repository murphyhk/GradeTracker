--create database universityGrades
use universityGrades


---		TABLES		---

-- Create student table
-- drop table student
create table student(
	studentID int NOT NULL,
	fname varchar(20) NOT NULL,
	lname varchar(20) NOT NULL,
	primary key (studentID)
)

select studentID from student where studentID = 1577713
select * from student where studentID = 1577713

-- Create course table
-- drop table course
create table course(
	courseCode varchar(10) NOT NULL,
	name varchar(100) NOT NULL,
	primary key(courseCode)
)

-- Create assignment table
-- drop table assignment
create table assignment(
	assName varchar(100) NOT NULL,
	percentageWorth real NOT NULL,
	course varchar(10) NOT NULL,
	primary key (assName, course),
	foreign key (course) references course
)

-- Create takePaper table
-- drop table takePaper
create table takePaper(
	studID int NOT NULL,
	papercode varchar(10) NOT NULL,
	grade real default 0,
	primary key (studID, papercode),
	foreign key (studID) references student,
	foreign key (papercode) references course,
)

select papercode, grade from takepaper where studID = 1577713

-- Create sit Assignment table
-- drop table sitAssignment
create table sitAssignment(
	studID int NOT NULL,
	assignmentName varchar(100) NOT NULL,
	course varchar(10) NOT NULL,
	earntGrade real default 0,
	percentEarnt real default 0,
	primary key (studID, assignmentName),
	foreign key (studID) references student,
	foreign key (assignmentName, course) references assignment
)

select assName, percentageWorth, earntGrade, percentEarnt 
from assignment a join sitAssignment s 
on a.assignmentID = s.assignmentID
where a.course = 'ENGEN201' and a.assName = 'Assignment 1'

select papercode, name, grade from takePaper t join course c
on t.papercode = c.courseCode where studID = 1577713 and c.courseCode = 'COMPX223'

---		INSERT STATEMENTS		---
insert into student values(1577713, 'Hannah', 'Murphy')
insert into course values('COMPX223', 'Database Practise and Experience')
insert into course values('COMPX203', 'Computer Systems')
insert into course values('COMPX241', 'Software Engineering Design 1')
insert into course values('ENGEN201', 'Engineering Maths and Modelling 2')

insert into assignment values('Mid Test 1', 20, 'COMPX241')
insert into assignment values('Mid Test 2', 20, 'COMPX241')

insert into takePaper(studID, papercode) values(1577713, 'COMPX223')
insert into takePaper(studID, papercode) values(1577713, 'COMPX203')
insert into takePaper(studID, papercode) values(1577713, 'COMPX241')
insert into takePaper(studID, papercode) values(1577713, 'ENGEN201')

insert into sitAssignment(studID, assignmentName, course) values(1577713, 'Final Exam', 'ENGEN201')
insert into sitAssignment(studID, assignmentName, course) values(1577713, 'Weekly Assignments', 'ENGEN201')

delete from assignment where assName = 'Test 2' and course = 'COMPX241'

select assName from assignment where course = 'COMPX223'

UPDATE sitAssignment SET assignmentName = 'assignment 2' WHERE studID = 1577713 and course = 'ENGEN201'

---		DISPLAY ALL		---
select * from student
select * from course
select * from takePaper
select * from assignment where course = 'COMPX241'

select * from sitAssignment


---		UPDATE ASSIGNMENT GRADE	---
drop trigger updateAssignmentGrade
Go
create trigger updateAssignmentGrade
	on sitAssignment
	for update, insert
	AS
	if @@rowcount = 0 return
	If exists (select * from inserted where earntGrade > 100) 
	begin
		raiserror('This is not a valid grade', 16,1)
		rollback transaction
	END
	If exists (select * from inserted where earntGrade < 0) 
	begin
		raiserror('This is not a valid grade', 16,1)
		rollback transaction
	END
go


-- Testing:
-- update sitAssignment set earntGrade = 66 where studID = 1577713 and assignmentID = 01;

---		UPDATE COURSE GRADE		---
drop trigger updateCourseGrade
Go
create trigger updateCourseGrade
	on takePaper
	for update, insert
	AS
	if @@rowcount = 0 return
	If exists (select * from inserted where grade > 100) 
	begin
		raiserror('This is not a valid grade', 16,1)
		rollback transaction
	END
	If exists (select * from inserted where grade < 0) 
	begin
		raiserror('This is not a valid grade', 16,1)
		rollback transaction
	END
go

-- Testing:
-- update takePaper set grade = 91 where studID = 1577713 and papercode = 'COMPX223';

---		UPDATE ASSIGNMENT PERCENTAGE GRADE	---
drop trigger updateAssignmentGradePercentage
Go
create trigger updateAssignmentGradePercentage
	on sitAssignment
	for update, insert
	AS
	if @@rowcount = 0 return
	If exists (select * from inserted where percentEarnt > 100) 
	begin
		raiserror('This is not a valid grade', 16,1)
		rollback transaction
	END
	If exists (select * from inserted where percentEarnt < 0) 
	begin
		raiserror('This is not a valid grade', 16,1)
		rollback transaction
	END
go


UPDATE sitAssignment SET earntGrade = 100 WHERE studID = 1577713 and assignmentName = 'Curriculum Quiz'


UPDATE sitAssignment SET earntGrade = 95 WHERE studID = 1577713 and assignmentName = 'Assignment 1'

-- assignmentName = 'Exam' and
select * from sitAssignment where course = 'COMPX203'
insert into sitAssignment values(1577713, 'Final Exam', 'COMPX203', 80, 0)

select * from assignment where assName = 'Final Exam' and course = 'COMPX203'
insert into assignment values('Final Exam', 50, 'COMPX203')

select earntGrade from sitAssignment where assignmentName = 'Exam' and course = 'COMPX203'
update sitAssignment set earntGrade = 80 where assignmentName = 'Exam' and course = 'COMPX203'

select percentageWorth from assignment where assName = 'Exam' and course = 'COMPX203'

update sitAssignment set percentEarnt = (select round(earntGrade* (percentageWorth/ 100), 2) as GradeWorth
    from sitAssignment s join assignment a on s.assignmentName = a.assName
    where s.studID = 1577713 and s.assignmentName = 'Final Exam' and s.course = 'COMPX203')
    where studID = 1577713 and assignmentName = 'Final Exam' and course = 'COMPX203'


insert into assignment values('Assignment 2', 1.67, 'ENGEN201')
insert into sitAssignment values(1577713, 'Test 1', 'COMPX241', 0, 0)

insert into assignment values('Assignment 3', 1.67, 'ENGEN201')
insert into sitAssignment values(1577713, 'Mid Test 1', 'COMPX241', 86.25, 0)

select * from takePaper

update takePaper set grade = (select sum(percentEarnt) from sitAssignment 
	where studID = 1577713 and course = 'ENGEN201')
	where studID = 1577713 and papercode = 'ENGEN201'
	
update takePaper set grade = (select sum(percentEarnt) from sitAssignment where studID = 1577713 and course = 'COMPX241') where studID = 1577713 and papercode = 'COMPX241'




select * from assignment where course = 'COMPX241'

select * from sitAssignment where course = 'COMPX241'
drop test1 from sitAssignment where course = 'COMPX241'

update sitAssignment set assignmentName = 'Mid Semester Test 1' where 