-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jun 30, 2025 at 01:12 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `attendancevarsity`
--

-- --------------------------------------------------------

--
-- Table structure for table `tbl_attendance`
--

CREATE TABLE `tbl_attendance` (
  `attendance_id` int(11) NOT NULL,
  `player_id` int(11) DEFAULT NULL,
  `session_id` int(11) DEFAULT NULL,
  `time_marked` datetime NOT NULL DEFAULT current_timestamp(),
  `image_path` varchar(255) DEFAULT NULL,
  `status` enum('Present','Absent','Pending') NOT NULL DEFAULT 'Pending',
  `checked_by` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_attendance`
--

INSERT INTO `tbl_attendance` (`attendance_id`, `player_id`, `session_id`, `time_marked`, `image_path`, `status`, `checked_by`) VALUES
(1, 5, 21, '2025-06-28 07:34:36', 'C:\\Users\\Acer\\Desktop\\AttendanceVarsity\\VarsityFaces\\222-2222\\attendance\\20250628_073436.jpg', 'Present', NULL),
(2, 5, 11, '2025-06-28 10:15:17', 'C:\\Users\\Acer\\Desktop\\AttendanceVarsity\\VarsityFaces\\222-2222\\Attendance\\20250628_101517.jpg', 'Present', NULL),
(3, 8, 23, '2025-06-29 14:19:53', 'C:\\Users\\Acer\\Desktop\\AttendanceVarsity\\VarsityFaces\\123-4321\\Attendance\\20250629_141953.jpg', 'Present', NULL),
(4, 9, 24, '2025-06-30 16:37:10', 'C:\\Users\\Acer\\Desktop\\AttendanceVarsity\\VarsityFaces\\191-0988\\Attendance\\20250630_163710.jpg', 'Present', NULL),
(5, 10, 24, '2025-06-30 16:38:45', 'C:\\Users\\Acer\\Desktop\\AttendanceVarsity\\VarsityFaces\\123-1344\\Attendance\\20250630_163845.jpg', 'Present', NULL),
(6, 8, 24, '2025-06-30 16:38:49', NULL, 'Absent', NULL),
(7, 11, 24, '2025-06-30 16:38:49', NULL, 'Absent', NULL),
(8, 8, 25, '2025-06-30 16:46:34', 'C:\\Users\\Acer\\Desktop\\AttendanceVarsity\\VarsityFaces\\123-4321\\Attendance\\20250630_164634.jpg', 'Present', NULL),
(9, 9, 25, '2025-06-30 16:46:36', NULL, 'Absent', NULL),
(10, 10, 25, '2025-06-30 16:46:36', NULL, 'Absent', NULL),
(11, 11, 25, '2025-06-30 16:46:36', NULL, 'Absent', NULL),
(12, 11, 26, '2025-06-30 17:24:42', 'C:\\Users\\Acer\\Desktop\\AttendanceVarsity\\VarsityFaces\\222-2221\\Attendance\\20250630_172442.jpg', 'Present', NULL),
(13, 8, 26, '2025-06-30 17:24:50', NULL, 'Absent', NULL),
(14, 9, 26, '2025-06-30 17:24:50', NULL, 'Absent', NULL),
(15, 10, 26, '2025-06-30 17:24:50', NULL, 'Absent', NULL),
(16, 11, 27, '2025-06-30 17:51:37', 'C:\\Users\\Acer\\Desktop\\AttendanceVarsity\\VarsityFaces\\222-2221\\Attendance\\20250630_175137.jpg', 'Present', NULL),
(17, 8, 27, '2025-06-30 17:51:43', 'C:\\Users\\Acer\\Desktop\\AttendanceVarsity\\VarsityFaces\\123-4321\\Attendance\\20250630_175143.jpg', 'Present', NULL),
(18, 9, 27, '2025-06-30 17:51:50', NULL, 'Absent', NULL),
(19, 10, 27, '2025-06-30 17:51:50', NULL, 'Absent', NULL),
(20, 8, 28, '2025-06-30 18:09:48', NULL, 'Absent', NULL),
(21, 9, 28, '2025-06-30 18:09:48', NULL, 'Absent', NULL),
(22, 10, 28, '2025-06-30 18:09:48', NULL, 'Absent', NULL),
(23, 11, 28, '2025-06-30 18:09:48', NULL, 'Absent', NULL),
(24, 11, 29, '2025-06-30 18:22:13', 'C:\\Users\\Acer\\Desktop\\AttendanceVarsity\\VarsityFaces\\222-2221\\Attendance\\20250630_182213.jpg', 'Present', NULL),
(25, 10, 30, '2025-06-30 18:32:20', 'C:\\Users\\Acer\\Desktop\\AttendanceVarsity\\VarsityFaces\\123-1344\\Attendance\\20250630_183219.jpg', 'Present', NULL),
(26, 8, 30, '2025-06-30 18:32:25', 'C:\\Users\\Acer\\Desktop\\AttendanceVarsity\\VarsityFaces\\123-4321\\Attendance\\20250630_183225.jpg', 'Present', NULL),
(27, 11, 30, '2025-06-30 18:32:28', 'C:\\Users\\Acer\\Desktop\\AttendanceVarsity\\VarsityFaces\\222-2221\\Attendance\\20250630_183228.jpg', 'Present', NULL),
(28, 9, 30, '2025-06-30 18:32:52', NULL, 'Absent', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_logs`
--

CREATE TABLE `tbl_logs` (
  `log_id` int(11) NOT NULL,
  `user_id` int(11) DEFAULT NULL,
  `action_type` varchar(50) DEFAULT NULL,
  `description` text DEFAULT NULL,
  `timestamp` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_players`
--

CREATE TABLE `tbl_players` (
  `player_id` int(11) NOT NULL,
  `student_id` varchar(20) NOT NULL,
  `full_name` varchar(100) NOT NULL,
  `gender` enum('Male','Female') NOT NULL,
  `level` enum('High School','College') NOT NULL,
  `sport_id` int(11) DEFAULT NULL,
  `face_folder_path` varchar(255) DEFAULT NULL,
  `date_registered` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_players`
--

INSERT INTO `tbl_players` (`player_id`, `student_id`, `full_name`, `gender`, `level`, `sport_id`, `face_folder_path`, `date_registered`) VALUES
(5, '222-2222', 'Reynaldo Dela Cruz', 'Male', 'High School', 1, 'C:\\Users\\Acer\\Desktop\\AttendanceVarsity\\VarsityFaces\\222-2222', '2025-06-28 07:34:26'),
(8, '123-4321', 'rd', 'Male', 'College', 2, 'C:\\Users\\Acer\\Desktop\\AttendanceVarsity\\VarsityFaces\\123-4321', '2025-06-29 14:19:35'),
(9, '191-0988', 'rd', 'Male', 'College', 2, 'C:\\Users\\Acer\\Desktop\\AttendanceVarsity\\VarsityFaces\\191-0988', '2025-06-30 14:55:28'),
(10, '123-1344', 'reynaldoi', 'Male', 'College', 2, 'C:\\Users\\Acer\\Desktop\\AttendanceVarsity\\VarsityFaces\\123-1344', '2025-06-30 15:46:57'),
(11, '222-2221', 'Reynaldoooo', 'Male', 'College', 2, 'C:\\Users\\Acer\\Desktop\\AttendanceVarsity\\VarsityFaces\\222-2221', '2025-06-30 15:51:52');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_sessions`
--

CREATE TABLE `tbl_sessions` (
  `session_id` int(11) NOT NULL,
  `session_title` varchar(100) DEFAULT NULL,
  `session_date` date NOT NULL,
  `level` enum('High School','College') DEFAULT NULL,
  `gender` enum('Male','Female','Both') NOT NULL,
  `sport_id` int(11) DEFAULT NULL,
  `created_by` int(11) DEFAULT NULL,
  `start_time` time NOT NULL,
  `end_time` time NOT NULL,
  `force_ended` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_sessions`
--

INSERT INTO `tbl_sessions` (`session_id`, `session_title`, `session_date`, `level`, `gender`, `sport_id`, `created_by`, `start_time`, `end_time`, `force_ended`) VALUES
(9, 'Volleyball - High School - Male - 2025-06-27 0000-1530', '2025-06-27', 'High School', 'Male', 2, NULL, '14:20:00', '15:30:00', 0),
(10, 'Volleyball - College - Male - 2025-06-26 0000-1431', '2025-06-26', 'College', 'Male', 2, NULL, '14:21:00', '14:31:00', 0),
(11, 'Basketball - High School - Male - 2025-06-28 0000-1200', '2025-06-28', 'High School', 'Male', 1, NULL, '10:00:00', '12:00:00', 0),
(12, 'Basketball - High School - Male - 2025-06-27 0000-1437', '2025-06-27', 'High School', 'Male', 1, NULL, '14:27:00', '14:37:00', 0),
(14, 'Basketball - High School - Male - 2025-06-27 0000-1442', '2025-06-27', 'High School', 'Male', 1, NULL, '14:32:00', '14:42:00', 0),
(15, 'Basketball - High School - Male - 2025-06-27 0000-1443', '2025-06-27', 'High School', 'Male', 1, NULL, '14:33:00', '14:43:00', 0),
(16, 'Basketball - High School - Male - 2025-06-27 0000-1930', '2025-06-27', 'High School', 'Male', 1, NULL, '17:00:00', '19:30:00', 0),
(17, 'Basketball - High School - Male - 2025-06-27 0000-1453', '2025-06-27', 'High School', 'Male', 1, NULL, '14:43:00', '14:53:00', 0),
(18, 'Basketball - High School - Male - 2025-06-27 0000-1743', '2025-06-27', 'High School', 'Male', 1, NULL, '17:33:00', '17:43:00', 0),
(19, 'Basketball - High School - Both - 2025-06-27 0000-2100', '2025-06-27', 'High School', 'Both', 1, NULL, '20:00:00', '21:00:00', 0),
(20, 'Basketball - College - Male - 2025-06-27 0000-2200', '2025-06-27', 'College', 'Male', 1, NULL, '20:00:00', '22:00:00', 0),
(21, 'Basketball - High School - Male - 2025-06-28 0000-0843', '2025-06-28', 'High School', 'Male', 1, NULL, '07:33:00', '08:43:00', 0),
(22, 'Volleyball - High School - Male - 2025-06-28 0000-1004', '2025-06-28', 'High School', 'Male', 2, NULL, '09:54:00', '10:04:00', 0),
(23, 'Volleyball - College - Male - 2025-06-29 0000-1423', '2025-06-29', 'College', 'Male', 2, NULL, '14:13:00', '14:23:00', 0),
(24, 'Volleyball - College - Male - 2025-06-30 0000-1645', '2025-06-30', 'College', 'Male', 2, NULL, '16:35:00', '16:45:00', 0),
(25, 'Volleyball - College - Male - 2025-06-30 0000-1656', '2025-06-30', 'College', 'Male', 2, NULL, '16:46:00', '16:56:00', 0),
(26, 'Volleyball - College - Male - 2025-06-30 0000-1734', '2025-06-30', 'College', 'Male', 2, NULL, '17:24:00', '17:24:50', 1),
(27, 'Volleyball - College - Male - 2025-06-30 0000-1801', '2025-06-30', 'College', 'Male', 2, NULL, '17:51:00', '17:51:50', 1),
(28, 'Volleyball - College - Male - 2025-06-30 0000-1819', '2025-06-30', 'College', 'Male', 2, NULL, '18:09:00', '18:09:48', 1),
(29, 'Volleyball - College - Male - 2025-06-30 0000-1829', '2025-06-30', 'College', 'Male', 2, NULL, '18:19:00', '18:29:00', 0),
(30, 'Volleyball - College - Male - 2025-06-30 0000-1842', '2025-06-30', 'College', 'Male', 2, NULL, '18:32:00', '18:32:52', 1);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_sports`
--

CREATE TABLE `tbl_sports` (
  `sport_id` int(11) NOT NULL,
  `sport_name` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_sports`
--

INSERT INTO `tbl_sports` (`sport_id`, `sport_name`) VALUES
(1, 'Basketball'),
(2, 'Volleyball');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_users`
--

CREATE TABLE `tbl_users` (
  `user_id` int(11) NOT NULL,
  `username` varchar(50) NOT NULL,
  `password_hash` varchar(255) NOT NULL,
  `full_name` varchar(100) DEFAULT NULL,
  `role` enum('Admin','Coach') DEFAULT 'Coach',
  `date_created` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_users`
--

INSERT INTO `tbl_users` (`user_id`, `username`, `password_hash`, `full_name`, `role`, `date_created`) VALUES
(1, 'admin', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 'System Administrator', 'Admin', '2025-06-26 19:06:27');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `tbl_attendance`
--
ALTER TABLE `tbl_attendance`
  ADD PRIMARY KEY (`attendance_id`),
  ADD UNIQUE KEY `uq_att` (`player_id`,`session_id`),
  ADD UNIQUE KEY `uq_session_player` (`session_id`,`player_id`),
  ADD KEY `fk_att_user` (`checked_by`);

--
-- Indexes for table `tbl_logs`
--
ALTER TABLE `tbl_logs`
  ADD PRIMARY KEY (`log_id`),
  ADD KEY `user_id` (`user_id`);

--
-- Indexes for table `tbl_players`
--
ALTER TABLE `tbl_players`
  ADD PRIMARY KEY (`player_id`),
  ADD UNIQUE KEY `student_id` (`student_id`),
  ADD KEY `sport_id` (`sport_id`);

--
-- Indexes for table `tbl_sessions`
--
ALTER TABLE `tbl_sessions`
  ADD PRIMARY KEY (`session_id`),
  ADD UNIQUE KEY `uq_slot` (`sport_id`,`level`,`gender`,`session_date`,`start_time`,`end_time`),
  ADD KEY `created_by` (`created_by`);

--
-- Indexes for table `tbl_sports`
--
ALTER TABLE `tbl_sports`
  ADD PRIMARY KEY (`sport_id`);

--
-- Indexes for table `tbl_users`
--
ALTER TABLE `tbl_users`
  ADD PRIMARY KEY (`user_id`),
  ADD UNIQUE KEY `username` (`username`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `tbl_attendance`
--
ALTER TABLE `tbl_attendance`
  MODIFY `attendance_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=29;

--
-- AUTO_INCREMENT for table `tbl_logs`
--
ALTER TABLE `tbl_logs`
  MODIFY `log_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbl_players`
--
ALTER TABLE `tbl_players`
  MODIFY `player_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT for table `tbl_sessions`
--
ALTER TABLE `tbl_sessions`
  MODIFY `session_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=31;

--
-- AUTO_INCREMENT for table `tbl_sports`
--
ALTER TABLE `tbl_sports`
  MODIFY `sport_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `tbl_users`
--
ALTER TABLE `tbl_users`
  MODIFY `user_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `tbl_attendance`
--
ALTER TABLE `tbl_attendance`
  ADD CONSTRAINT `fk_att_user` FOREIGN KEY (`checked_by`) REFERENCES `tbl_users` (`user_id`),
  ADD CONSTRAINT `tbl_attendance_ibfk_1` FOREIGN KEY (`player_id`) REFERENCES `tbl_players` (`player_id`),
  ADD CONSTRAINT `tbl_attendance_ibfk_2` FOREIGN KEY (`session_id`) REFERENCES `tbl_sessions` (`session_id`);

--
-- Constraints for table `tbl_logs`
--
ALTER TABLE `tbl_logs`
  ADD CONSTRAINT `tbl_logs_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `tbl_users` (`user_id`);

--
-- Constraints for table `tbl_players`
--
ALTER TABLE `tbl_players`
  ADD CONSTRAINT `tbl_players_ibfk_1` FOREIGN KEY (`sport_id`) REFERENCES `tbl_sports` (`sport_id`);

--
-- Constraints for table `tbl_sessions`
--
ALTER TABLE `tbl_sessions`
  ADD CONSTRAINT `tbl_sessions_ibfk_1` FOREIGN KEY (`sport_id`) REFERENCES `tbl_sports` (`sport_id`),
  ADD CONSTRAINT `tbl_sessions_ibfk_2` FOREIGN KEY (`created_by`) REFERENCES `tbl_users` (`user_id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
