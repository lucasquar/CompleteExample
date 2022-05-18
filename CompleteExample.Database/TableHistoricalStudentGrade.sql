CREATE TABLE [dbo].[HistoricalStudentGrades]
(
	HistoricalId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	StudentId INT NOT NULL,
	CourseId INT NOT NULL,
	Grade DECIMAL(5,2) NULL,
    GradeDate DATETIME NOT NULL,
    CONSTRAINT [FK_HistoricalStudentGrade_Students] FOREIGN KEY ([StudentId]) REFERENCES [Students]([StudentId]),
	CONSTRAINT [FK_HistoricalStudentGrade_Courses] FOREIGN KEY ([CourseId]) REFERENCES [Courses]([CourseId])
);