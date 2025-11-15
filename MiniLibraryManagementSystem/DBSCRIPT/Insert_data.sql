use MiniLibaryManagementSystem;
GO
INSERT INTO BookManagementTable (TITLE, AUTHOR, ISBN, CATEGORY, COPIESAVAILABLE, PUBLISHEDYEAR, STATUS)
VALUES
('The Pragmatic Programmer', 'Andrew Hunt', '978-0201616224', 'Programming', 5, 1999, 1),
('Clean Code', 'Robert C. Martin', '978-0132350884', 'Programming', 8, 2008, 1),
('Introduction to Algorithms', 'Thomas H. Cormen', '978-0262033848', 'Computer Science', 3, 2009, 1),
('Atomic Habits', 'James Clear', '978-0735211292', 'Self Help', 10, 2018, 1),
('Deep Work', 'Cal Newport', '978-1455586691', 'Productivity', 4, 2016, 1),
('The Psychology of Money', 'Morgan Housel', '978-0857197689', 'Finance', 6, 2020, 1),
('The Art of War', 'Sun Tzu', '978-1590302255', 'Philosophy', 2, 2005, 1),
('Design Patterns', 'Erich Gamma', '978-0201633610', 'Software Engineering', 7, 1994, 1),
('The Great Gatsby', 'F. Scott Fitzgerald', '978-0743273565', 'Fiction', 9, 1925, 1),
('1984', 'George Orwell', '978-0451524935', 'Fiction', 5, 1949, 1);
GO

INSERT INTO MemberManagementTable (FULLNAME, EMAIL, PHONE, JOINDATE, ISACTIVE)
VALUES
('Alice Johnson', 'alice.johnson@example.com', '01710000001', '2023-01-10', 1),
('Bob Smith', 'bob.smith@example.com', '01710000002', '2023-02-15', 1),
('Charlie Davis', 'charlie.davis@example.com', '01710000003', '2023-03-05', 1),
('Diana Prince', 'diana.prince@example.com', '01710000004', '2023-04-20', 1),
('Edward Brown', 'edward.brown@example.com', '01710000005', '2023-05-25', 1),
('Fiona Wilson', 'fiona.wilson@example.com', '01710000006', '2023-06-30', 1),
('George Martin', 'george.martin@example.com', '01710000007', '2023-07-12', 1),
('Hannah Lee', 'hannah.lee@example.com', '01710000008', '2023-08-08', 1),
('Ian Clark', 'ian.clark@example.com', '01710000009', '2023-09-14', 1),
('Julia Roberts', 'julia.roberts@example.com', '01710000010', '2023-10-21', 1);
GO

INSERT INTO BorrowDetailsTable (MEMBERID, BORROWDATE, DUEDATE, RETURNDATE)
VALUES
(1, '2024-01-10', '2024-01-20', '2024-01-18'),
(2, '2024-02-05', '2024-02-15', '2024-02-14'),
(3, '2024-03-12', '2024-03-22', NULL),
(4, '2024-04-01', '2024-04-10', '2024-04-08'),
(5, '2024-05-05', '2024-05-15', NULL),
(6, '2024-06-09', '2024-06-19', '2024-06-17'),
(7, '2024-07-15', '2024-07-25', NULL),
(8, '2024-08-20', '2024-08-30', '2024-08-28'),
(9, '2024-09-10', '2024-09-20', NULL),
(10, '2024-10-05', '2024-10-15', '2024-10-14');
GO

-- BorrowId 1 → 2 books
INSERT INTO BorrowBookListTable (BORROWID, BOOKID) VALUES
(1, 1),
(1, 2);

-- BorrowId 2 → 3 books
INSERT INTO BorrowBookListTable (BORROWID, BOOKID) VALUES
(2, 3),
(2, 4),
(2, 5);

-- BorrowId 3 → 2 books
INSERT INTO BorrowBookListTable (BORROWID, BOOKID) VALUES
(3, 6),
(3, 7);

-- BorrowId 4 → 3 books
INSERT INTO BorrowBookListTable (BORROWID, BOOKID) VALUES
(4, 8),
(4, 9),
(4, 10);

-- BorrowId 5 → 2 books
INSERT INTO BorrowBookListTable (BORROWID, BOOKID) VALUES
(5, 1),
(5, 3);

-- BorrowId 6 → 3 books
INSERT INTO BorrowBookListTable (BORROWID, BOOKID) VALUES
(6, 2),
(6, 5),
(6, 8);

-- BorrowId 7 → 2 books
INSERT INTO BorrowBookListTable (BORROWID, BOOKID) VALUES
(7, 4),
(7, 7);

-- BorrowId 8 → 3 books
INSERT INTO BorrowBookListTable (BORROWID, BOOKID) VALUES
(8, 6),
(8, 9),
(8, 10);

-- BorrowId 9 → 3 books
INSERT INTO BorrowBookListTable (BORROWID, BOOKID) VALUES
(9, 1),
(9, 5),
(9, 7);

-- BorrowId 10 → 2 books
INSERT INTO BorrowBookListTable (BORROWID, BOOKID) VALUES
(10, 2),
(10, 3);
GO
