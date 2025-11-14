-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Máy chủ: 127.0.0.1
-- Thời gian đã tạo: Th10 14, 2025 lúc 09:48 AM
-- Phiên bản máy phục vụ: 10.4.32-MariaDB
-- Phiên bản PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Cơ sở dữ liệu: `quan_cafe`
--

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `ban`
--

CREATE TABLE `ban` (
  `MABAN` int(11) NOT NULL,
  `TENBAN` varchar(100) NOT NULL,
  `DANGSUDUNG` tinyint(1) NOT NULL DEFAULT 1,
  `MADONHIENTAI` int(11) DEFAULT NULL,
  `MAKHUVUC` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `ban`
--

INSERT INTO `ban` (`MABAN`, `TENBAN`, `DANGSUDUNG`, `MADONHIENTAI`, `MAKHUVUC`) VALUES
(1, 'Ban1', 1, NULL, 1),
(2, 'Ban2', 1, NULL, 1),
(3, 'Ban3', 1, NULL, 1),
(4, 'Ban4', 1, NULL, 1),
(5, 'Ban5', 1, NULL, 1),
(6, 'Ban6', 1, NULL, 1),
(7, 'Ban7', 1, NULL, 1),
(8, 'Ban8', 1, NULL, 1),
(9, 'Ban9', 1, NULL, 1),
(10, 'Ban10', 1, NULL, 1),
(11, 'Ban11', 1, NULL, 1),
(12, 'Ban12', 1, NULL, 1),
(13, 'Ban13', 1, NULL, 1),
(14, 'Ban14', 1, NULL, 1),
(15, 'Ban15', 1, NULL, 1),
(16, 'Ban16', 1, NULL, 1),
(17, 'Ban17', 1, NULL, 1),
(18, 'Ban18', 1, NULL, 1),
(19, 'Ban19', 1, NULL, 1),
(20, 'Ban20', 1, NULL, 1),
(21, 'Ban21', 1, NULL, 2),
(22, 'Ban22', 1, NULL, 2),
(23, 'Ban23', 1, NULL, 2),
(24, 'Ban24', 1, NULL, 2),
(25, 'Ban25', 1, NULL, 2),
(26, 'Ban26', 1, NULL, 2),
(27, 'Ban27', 1, NULL, 2),
(28, 'Ban28', 1, NULL, 2),
(29, 'Ban29', 1, NULL, 2),
(30, 'Ban30', 1, NULL, 2),
(31, 'Ban31', 1, NULL, 2),
(32, 'Ban32', 1, NULL, 2),
(33, 'Ban33', 1, NULL, 2),
(34, 'Ban34', 1, NULL, 2),
(35, 'Ban35', 1, NULL, 2),
(36, 'Ban36', 1, NULL, 2),
(37, 'Ban37', 1, NULL, 2),
(38, 'Ban38', 1, NULL, 2),
(39, 'Ban39', 1, NULL, 2),
(40, 'Ban40', 1, NULL, 2);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `calam`
--

CREATE TABLE `calam` (
  `MACA` int(11) NOT NULL,
  `TENCA` varchar(60) NOT NULL,
  `THOIGIANBD` time NOT NULL,
  `THOIGIANKT` time NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `congthuc`
--

CREATE TABLE `congthuc` (
  `MASANPHAM` int(11) NOT NULL,
  `MANGUYENLIEU` int(11) NOT NULL,
  `SOLUONGCOSO` decimal(12,2) NOT NULL,
  `MADONVICOSO` int(11) NOT NULL,
  `TRANGTHAI` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `congthuc`
--

INSERT INTO `congthuc` (`MASANPHAM`, `MANGUYENLIEU`, `SOLUONGCOSO`, `MADONVICOSO`, `TRANGTHAI`) VALUES
(1, 1, 25.00, 1, 1),
(1, 2, 5.00, 1, 1),
(2, 1, 25.00, 1, 1),
(2, 3, 40.00, 1, 1),
(3, 1, 20.00, 1, 1),
(3, 2, 5.00, 1, 1),
(3, 3, 25.00, 1, 1),
(3, 4, 120.00, 3, 1),
(4, 5, 1.00, 5, 1);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `cthd`
--

CREATE TABLE `cthd` (
  `MAHOADON` int(11) NOT NULL,
  `MASANPHAM` int(11) NOT NULL,
  `SOLUONG` int(11) NOT NULL CHECK (`SOLUONG` > 0),
  `DONGIA` decimal(12,2) NOT NULL,
  `THANHTIEN` decimal(12,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `cthd`
--

INSERT INTO `cthd` (`MAHOADON`, `MASANPHAM`, `SOLUONG`, `DONGIA`, `THANHTIEN`) VALUES
(6, 1, 1, 17000.00, 17000.00),
(6, 2, 1, 25000.00, 25000.00),
(6, 3, 1, 25000.00, 25000.00),
(15, 1, 1, 17000.00, 17000.00),
(17, 1, 1, 17000.00, 17000.00),
(18, 2, 2, 25000.00, 50000.00),
(18, 3, 2, 25000.00, 50000.00),
(19, 1, 3, 17000.00, 51000.00),
(19, 2, 5, 25000.00, 125000.00),
(19, 5, 1, 2000000.00, 2000000.00);

--
-- Bẫy `cthd`
--
DELIMITER $$
CREATE TRIGGER `trg_cthd_ad` AFTER DELETE ON `cthd` FOR EACH ROW BEGIN
  UPDATE HOADON SET TONGTIEN = TONGTIEN - OLD.THANHTIEN WHERE MAHOADON = OLD.MAHOADON;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `trg_cthd_ai` AFTER INSERT ON `cthd` FOR EACH ROW BEGIN
  UPDATE HOADON SET TONGTIEN = TONGTIEN + NEW.THANHTIEN WHERE MAHOADON = NEW.MAHOADON;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `ctphieunhap`
--

CREATE TABLE `ctphieunhap` (
  `MACTPN` int(11) NOT NULL,
  `MAPN` int(11) DEFAULT NULL,
  `MANGUYENLIEU` int(11) DEFAULT NULL,
  `MADONVI` int(11) DEFAULT NULL,
  `SOLUONG` decimal(10,2) DEFAULT NULL,
  `SOLUONGCOSO` decimal(10,2) DEFAULT NULL,
  `DONGIA` decimal(10,2) DEFAULT NULL,
  `THANHTIEN` decimal(10,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `donvi`
--

CREATE TABLE `donvi` (
  `MADONVI` int(11) NOT NULL,
  `TENDONVI` varchar(50) NOT NULL,
  `TRANGTHAI` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `donvi`
--

INSERT INTO `donvi` (`MADONVI`, `TENDONVI`, `TRANGTHAI`) VALUES
(1, 'g', 1),
(2, 'kg', 1),
(3, 'ml', 1),
(4, 'lít', 1),
(5, 'lon', 1),
(6, 'chai', 1),
(7, 'lốc', 1),
(8, 'thùng', 1),
(9, 'hộp', 1);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `hesodonvi`
--

CREATE TABLE `hesodonvi` (
  `MANGUYENLIEU` int(11) NOT NULL,
  `MADONVI` int(11) NOT NULL,
  `HESO` decimal(10,3) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `hesodonvi`
--

INSERT INTO `hesodonvi` (`MANGUYENLIEU`, `MADONVI`, `HESO`) VALUES
(1, 2, 1.000),
(2, 2, 1.000),
(3, 9, 1.000),
(4, 9, 1.000),
(5, 7, 6.000),
(5, 8, 24.000);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `hoadon`
--

CREATE TABLE `hoadon` (
  `MAHOADON` int(11) NOT NULL,
  `MABAN` int(11) DEFAULT NULL,
  `MATT` int(11) DEFAULT NULL,
  `THOIGIANTAO` datetime NOT NULL DEFAULT current_timestamp(),
  `TRANGTHAI` tinyint(1) NOT NULL DEFAULT 0,
  `TONGTIEN` decimal(12,2) NOT NULL DEFAULT 0.00,
  `MAKHACHHANG` int(11) DEFAULT NULL,
  `MANHANVIEN` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `hoadon`
--

INSERT INTO `hoadon` (`MAHOADON`, `MABAN`, `MATT`, `THOIGIANTAO`, `TRANGTHAI`, `TONGTIEN`, `MAKHACHHANG`, `MANHANVIEN`) VALUES
(6, 1, 1, '2025-11-14 13:57:53', 0, 67000.00, 1, 1),
(15, 1, 1, '2025-11-14 14:44:44', 0, 17000.00, 1, 1),
(17, 40, 1, '2025-11-14 15:07:45', 0, 17000.00, 1, 1),
(18, 8, 1, '2025-11-14 15:24:58', 0, 100000.00, 1, 1),
(19, 8, 1, '2025-11-14 15:38:52', 0, 2176000.00, 1, 1);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `khachhang`
--

CREATE TABLE `khachhang` (
  `MAKHACHHANG` int(11) NOT NULL,
  `TENKHACHHANG` varchar(120) NOT NULL,
  `SODIENTHOAI` varchar(20) DEFAULT NULL,
  `EMAIL` varchar(120) DEFAULT NULL,
  `TRANGTHAI` tinyint(1) NOT NULL DEFAULT 1,
  `NGAYTAO` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `khachhang`
--

INSERT INTO `khachhang` (`MAKHACHHANG`, `TENKHACHHANG`, `SODIENTHOAI`, `EMAIL`, `TRANGTHAI`, `NGAYTAO`) VALUES
(1, 'abc', '113', 'm@gmail.com', 1, '2025-11-14 13:17:26');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `khuvuc`
--

CREATE TABLE `khuvuc` (
  `MAKHUVUC` int(11) NOT NULL,
  `TENKHUVUC` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `khuvuc`
--

INSERT INTO `khuvuc` (`MAKHUVUC`, `TENKHUVUC`) VALUES
(1, 'A'),
(2, 'B');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `lichlamviec`
--

CREATE TABLE `lichlamviec` (
  `MANHANVIEN` int(11) NOT NULL,
  `NGAY` date NOT NULL,
  `MACA` int(11) NOT NULL,
  `TRANGTHAI` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `loai`
--

CREATE TABLE `loai` (
  `MALOAI` int(11) NOT NULL,
  `MANHOM` int(11) DEFAULT NULL,
  `TENLOAI` varchar(100) NOT NULL,
  `TRANGTHAI` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `loai`
--

INSERT INTO `loai` (`MALOAI`, `MANHOM`, `TENLOAI`, `TRANGTHAI`) VALUES
(1, 1, 'Cà phê', 1),
(2, 1, 'Nước ngọt', 1);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `nguyenlieu`
--

CREATE TABLE `nguyenlieu` (
  `MANGUYENLIEU` int(11) NOT NULL,
  `TENNGUYENLIEU` varchar(120) NOT NULL,
  `TRANGTHAI` tinyint(1) NOT NULL DEFAULT 1,
  `TONKHO` decimal(14,3) NOT NULL DEFAULT 0.000,
  `TRANGTHAIDV` tinyint(1) NOT NULL DEFAULT 0,
  `MADONVICOSO` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `nguyenlieu`
--

INSERT INTO `nguyenlieu` (`MANGUYENLIEU`, `TENNGUYENLIEU`, `TRANGTHAI`, `TONKHO`, `TRANGTHAIDV`, `MADONVICOSO`) VALUES
(1, 'Bột cà phê', 1, 0.000, 1, 2),
(2, 'Đường trắng', 1, 0.000, 1, 2),
(3, 'Sữa đặc', 1, 0.000, 1, 2),
(4, 'Sữa tươi không đường', 1, 0.000, 1, 4),
(5, 'Coca cola', 1, 0.000, 1, 5);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `nhacungcap`
--

CREATE TABLE `nhacungcap` (
  `MANCC` int(11) NOT NULL,
  `TENNCC` varchar(150) NOT NULL,
  `SODIENTHOAI` varchar(20) DEFAULT NULL,
  `EMAIL` varchar(120) DEFAULT NULL,
  `DIACHI` varchar(255) DEFAULT NULL,
  `CONHOATDONG` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `nhanvien`
--

CREATE TABLE `nhanvien` (
  `MANHANVIEN` int(11) NOT NULL,
  `HOTEN` varchar(120) NOT NULL,
  `SODIENTHOAI` varchar(20) DEFAULT NULL,
  `EMAIL` varchar(120) DEFAULT NULL,
  `LUONG` decimal(12,2) DEFAULT 0.00,
  `NGAYTAO` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `nhanvien`
--

INSERT INTO `nhanvien` (`MANHANVIEN`, `HOTEN`, `SODIENTHOAI`, `EMAIL`, `LUONG`, `NGAYTAO`) VALUES
(1, 'bcd', '12312', '12123@gmail.com', 10000.00, '2025-11-14 13:17:41');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `nhom`
--

CREATE TABLE `nhom` (
  `MANHOM` int(11) NOT NULL,
  `TENNHOM` varchar(100) NOT NULL,
  `TRANGTHAI` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `nhom`
--

INSERT INTO `nhom` (`MANHOM`, `TENNHOM`, `TRANGTHAI`) VALUES
(1, 'Đồ uống', 1),
(2, 'Đồ ăn', 1),
(3, 'Khác', 1);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `phanquyen`
--

CREATE TABLE `phanquyen` (
  `MAVAITRO` int(11) NOT NULL,
  `MAQUYEN` int(11) NOT NULL,
  `CAN_READ` tinyint(1) DEFAULT 0,
  `CAN_WRITE` tinyint(1) DEFAULT 0,
  `CAN_UPDATE` tinyint(1) DEFAULT 0,
  `CAN_DELETE` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `phanquyen`
--

INSERT INTO `phanquyen` (`MAVAITRO`, `MAQUYEN`, `CAN_READ`, `CAN_WRITE`, `CAN_UPDATE`, `CAN_DELETE`) VALUES
(1, 1, 1, 1, 1, 1),
(1, 2, 1, 1, 1, 1),
(1, 3, 1, 1, 1, 1),
(2, 1, 1, 0, 0, 0),
(2, 2, 1, 1, 0, 0);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `phieunhap`
--

CREATE TABLE `phieunhap` (
  `MAPN` int(11) NOT NULL,
  `MANCC` int(11) NOT NULL,
  `THOIGIAN` datetime NOT NULL DEFAULT current_timestamp(),
  `TRANGTHAI` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `quyen`
--

CREATE TABLE `quyen` (
  `MAQUYEN` int(11) NOT NULL,
  `TENQUYEN` varchar(100) NOT NULL,
  `TRANGTHAI` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `quyen`
--

INSERT INTO `quyen` (`MAQUYEN`, `TENQUYEN`, `TRANGTHAI`) VALUES
(1, 'Quản lý sản phẩm', 1),
(2, 'Nhập xuất', 1),
(3, 'Thanh toán', 1);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `sanpham`
--

CREATE TABLE `sanpham` (
  `MASANPHAM` int(11) NOT NULL,
  `MALOAI` int(11) NOT NULL,
  `HINH` varchar(255) DEFAULT NULL,
  `TENSANPHAM` varchar(120) NOT NULL,
  `TRANGTHAI` tinyint(1) NOT NULL DEFAULT 1,
  `TRANGTHAICT` tinyint(1) NOT NULL DEFAULT 0,
  `GIA` decimal(12,2) NOT NULL DEFAULT 0.00
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `sanpham`
--

INSERT INTO `sanpham` (`MASANPHAM`, `MALOAI`, `HINH`, `TENSANPHAM`, `TRANGTHAI`, `TRANGTHAICT`, `GIA`) VALUES
(1, 1, 'cafe-den-da-8801.png', 'Cà phê đen', 1, 1, 17000.00),
(2, 1, 'pngtree-ice-milk-coffee-png-image_9162395.png', 'Cà phê sữa', 1, 1, 25000.00),
(3, 1, 'pngtree-cute-iced-coffee-takeaway-png-image_11477425.png', 'Cà phê sữa tươi', 1, 1, 25000.00),
(4, 2, 's-1-lon-cocacola.png', 'Coca cola lon', 1, 1, 15000.00),
(5, 1, 'sp_20251114153830_4b348a.jpg', 'abc', 0, 0, 2000000.00);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `taikhoan`
--

CREATE TABLE `taikhoan` (
  `MATAIKHOAN` int(11) NOT NULL,
  `MANHANVIEN` int(11) NOT NULL,
  `TENDANGNHAP` varchar(60) NOT NULL,
  `MATKHAU` varchar(255) NOT NULL,
  `TRANGTHAI` tinyint(1) NOT NULL DEFAULT 1,
  `NGAYTAO` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `thanhtoan`
--

CREATE TABLE `thanhtoan` (
  `MATT` int(11) NOT NULL,
  `HINHTHUC` varchar(60) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `thanhtoan`
--

INSERT INTO `thanhtoan` (`MATT`, `HINHTHUC`) VALUES
(1, 'CK');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `vaitro`
--

CREATE TABLE `vaitro` (
  `MAVAITRO` int(11) NOT NULL,
  `TENVAITRO` varchar(60) NOT NULL,
  `TRANGTHAI` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `vaitro`
--

INSERT INTO `vaitro` (`MAVAITRO`, `TENVAITRO`, `TRANGTHAI`) VALUES
(1, 'Admin', 1),
(2, 'Nhân viên', 1),
(3, 'Khách hàng', 1);

--
-- Chỉ mục cho các bảng đã đổ
--

--
-- Chỉ mục cho bảng `ban`
--
ALTER TABLE `ban`
  ADD PRIMARY KEY (`MABAN`),
  ADD KEY `FK_BAN_KHUVUC` (`MAKHUVUC`);

--
-- Chỉ mục cho bảng `calam`
--
ALTER TABLE `calam`
  ADD PRIMARY KEY (`MACA`);

--
-- Chỉ mục cho bảng `congthuc`
--
ALTER TABLE `congthuc`
  ADD PRIMARY KEY (`MASANPHAM`,`MANGUYENLIEU`,`MADONVICOSO`),
  ADD KEY `FK_CT_NL` (`MANGUYENLIEU`),
  ADD KEY `fk_congthuc_donvi` (`MADONVICOSO`);

--
-- Chỉ mục cho bảng `cthd`
--
ALTER TABLE `cthd`
  ADD PRIMARY KEY (`MAHOADON`,`MASANPHAM`),
  ADD KEY `FK_CTHD_SP` (`MASANPHAM`);

--
-- Chỉ mục cho bảng `ctphieunhap`
--
ALTER TABLE `ctphieunhap`
  ADD PRIMARY KEY (`MACTPN`),
  ADD KEY `MAPN` (`MAPN`),
  ADD KEY `MANGUYENLIEU` (`MANGUYENLIEU`),
  ADD KEY `MADONVI` (`MADONVI`);

--
-- Chỉ mục cho bảng `donvi`
--
ALTER TABLE `donvi`
  ADD PRIMARY KEY (`MADONVI`);

--
-- Chỉ mục cho bảng `hesodonvi`
--
ALTER TABLE `hesodonvi`
  ADD PRIMARY KEY (`MANGUYENLIEU`,`MADONVI`),
  ADD KEY `MADONVI` (`MADONVI`);

--
-- Chỉ mục cho bảng `hoadon`
--
ALTER TABLE `hoadon`
  ADD PRIMARY KEY (`MAHOADON`),
  ADD KEY `FK_HD_BAN` (`MABAN`),
  ADD KEY `FK_HD_TT` (`MATT`),
  ADD KEY `FK_HD_KH` (`MAKHACHHANG`),
  ADD KEY `FK_HD_NV` (`MANHANVIEN`),
  ADD KEY `idx_hd_time` (`THOIGIANTAO`);

--
-- Chỉ mục cho bảng `khachhang`
--
ALTER TABLE `khachhang`
  ADD PRIMARY KEY (`MAKHACHHANG`),
  ADD UNIQUE KEY `uq_kh_sdt` (`SODIENTHOAI`),
  ADD UNIQUE KEY `uq_kh_email` (`EMAIL`);

--
-- Chỉ mục cho bảng `khuvuc`
--
ALTER TABLE `khuvuc`
  ADD PRIMARY KEY (`MAKHUVUC`),
  ADD UNIQUE KEY `uq_khuvuc_ten` (`TENKHUVUC`);

--
-- Chỉ mục cho bảng `lichlamviec`
--
ALTER TABLE `lichlamviec`
  ADD PRIMARY KEY (`MANHANVIEN`,`NGAY`,`MACA`),
  ADD KEY `FK_LLV_CA` (`MACA`);

--
-- Chỉ mục cho bảng `loai`
--
ALTER TABLE `loai`
  ADD PRIMARY KEY (`MALOAI`),
  ADD UNIQUE KEY `uq_loai_ten` (`TENLOAI`),
  ADD KEY `FK_LOAI_NHOM` (`MANHOM`);

--
-- Chỉ mục cho bảng `nguyenlieu`
--
ALTER TABLE `nguyenlieu`
  ADD PRIMARY KEY (`MANGUYENLIEU`),
  ADD UNIQUE KEY `uq_nl_ten` (`TENNGUYENLIEU`),
  ADD KEY `MADONVICOSO` (`MADONVICOSO`);

--
-- Chỉ mục cho bảng `nhacungcap`
--
ALTER TABLE `nhacungcap`
  ADD PRIMARY KEY (`MANCC`);

--
-- Chỉ mục cho bảng `nhanvien`
--
ALTER TABLE `nhanvien`
  ADD PRIMARY KEY (`MANHANVIEN`),
  ADD UNIQUE KEY `uq_nhanvien_email` (`EMAIL`);

--
-- Chỉ mục cho bảng `nhom`
--
ALTER TABLE `nhom`
  ADD PRIMARY KEY (`MANHOM`);

--
-- Chỉ mục cho bảng `phanquyen`
--
ALTER TABLE `phanquyen`
  ADD PRIMARY KEY (`MAVAITRO`,`MAQUYEN`),
  ADD KEY `MAQUYEN` (`MAQUYEN`);

--
-- Chỉ mục cho bảng `phieunhap`
--
ALTER TABLE `phieunhap`
  ADD PRIMARY KEY (`MAPN`),
  ADD KEY `FK_PN_NCC` (`MANCC`);

--
-- Chỉ mục cho bảng `quyen`
--
ALTER TABLE `quyen`
  ADD PRIMARY KEY (`MAQUYEN`);

--
-- Chỉ mục cho bảng `sanpham`
--
ALTER TABLE `sanpham`
  ADD PRIMARY KEY (`MASANPHAM`),
  ADD KEY `FK_SANPHAM_LOAI` (`MALOAI`),
  ADD KEY `idx_sp_ten` (`TENSANPHAM`);

--
-- Chỉ mục cho bảng `taikhoan`
--
ALTER TABLE `taikhoan`
  ADD PRIMARY KEY (`MATAIKHOAN`),
  ADD UNIQUE KEY `MANHANVIEN` (`MANHANVIEN`),
  ADD UNIQUE KEY `TENDANGNHAP` (`TENDANGNHAP`);

--
-- Chỉ mục cho bảng `thanhtoan`
--
ALTER TABLE `thanhtoan`
  ADD PRIMARY KEY (`MATT`);

--
-- Chỉ mục cho bảng `vaitro`
--
ALTER TABLE `vaitro`
  ADD PRIMARY KEY (`MAVAITRO`),
  ADD UNIQUE KEY `TENVAITRO` (`TENVAITRO`);

--
-- AUTO_INCREMENT cho các bảng đã đổ
--

--
-- AUTO_INCREMENT cho bảng `ban`
--
ALTER TABLE `ban`
  MODIFY `MABAN` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=41;

--
-- AUTO_INCREMENT cho bảng `calam`
--
ALTER TABLE `calam`
  MODIFY `MACA` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `ctphieunhap`
--
ALTER TABLE `ctphieunhap`
  MODIFY `MACTPN` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `donvi`
--
ALTER TABLE `donvi`
  MODIFY `MADONVI` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT cho bảng `hoadon`
--
ALTER TABLE `hoadon`
  MODIFY `MAHOADON` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;

--
-- AUTO_INCREMENT cho bảng `khachhang`
--
ALTER TABLE `khachhang`
  MODIFY `MAKHACHHANG` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT cho bảng `khuvuc`
--
ALTER TABLE `khuvuc`
  MODIFY `MAKHUVUC` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT cho bảng `loai`
--
ALTER TABLE `loai`
  MODIFY `MALOAI` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT cho bảng `nguyenlieu`
--
ALTER TABLE `nguyenlieu`
  MODIFY `MANGUYENLIEU` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT cho bảng `nhacungcap`
--
ALTER TABLE `nhacungcap`
  MODIFY `MANCC` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `nhanvien`
--
ALTER TABLE `nhanvien`
  MODIFY `MANHANVIEN` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT cho bảng `nhom`
--
ALTER TABLE `nhom`
  MODIFY `MANHOM` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT cho bảng `phieunhap`
--
ALTER TABLE `phieunhap`
  MODIFY `MAPN` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `quyen`
--
ALTER TABLE `quyen`
  MODIFY `MAQUYEN` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT cho bảng `sanpham`
--
ALTER TABLE `sanpham`
  MODIFY `MASANPHAM` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT cho bảng `taikhoan`
--
ALTER TABLE `taikhoan`
  MODIFY `MATAIKHOAN` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `thanhtoan`
--
ALTER TABLE `thanhtoan`
  MODIFY `MATT` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT cho bảng `vaitro`
--
ALTER TABLE `vaitro`
  MODIFY `MAVAITRO` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Các ràng buộc cho các bảng đã đổ
--

--
-- Các ràng buộc cho bảng `ban`
--
ALTER TABLE `ban`
  ADD CONSTRAINT `FK_BAN_KHUVUC` FOREIGN KEY (`MAKHUVUC`) REFERENCES `khuvuc` (`MAKHUVUC`);

--
-- Các ràng buộc cho bảng `congthuc`
--
ALTER TABLE `congthuc`
  ADD CONSTRAINT `FK_CT_NL` FOREIGN KEY (`MANGUYENLIEU`) REFERENCES `nguyenlieu` (`MANGUYENLIEU`),
  ADD CONSTRAINT `FK_CT_SP` FOREIGN KEY (`MASANPHAM`) REFERENCES `sanpham` (`MASANPHAM`),
  ADD CONSTRAINT `fk_congthuc_donvi` FOREIGN KEY (`MADONVICOSO`) REFERENCES `donvi` (`MADONVI`);

--
-- Các ràng buộc cho bảng `cthd`
--
ALTER TABLE `cthd`
  ADD CONSTRAINT `FK_CTHD_HD` FOREIGN KEY (`MAHOADON`) REFERENCES `hoadon` (`MAHOADON`),
  ADD CONSTRAINT `FK_CTHD_SP` FOREIGN KEY (`MASANPHAM`) REFERENCES `sanpham` (`MASANPHAM`);

--
-- Các ràng buộc cho bảng `ctphieunhap`
--
ALTER TABLE `ctphieunhap`
  ADD CONSTRAINT `ctphieunhap_ibfk_1` FOREIGN KEY (`MAPN`) REFERENCES `phieunhap` (`MAPN`),
  ADD CONSTRAINT `ctphieunhap_ibfk_2` FOREIGN KEY (`MANGUYENLIEU`) REFERENCES `nguyenlieu` (`MANGUYENLIEU`),
  ADD CONSTRAINT `ctphieunhap_ibfk_3` FOREIGN KEY (`MADONVI`) REFERENCES `donvi` (`MADONVI`);

--
-- Các ràng buộc cho bảng `hesodonvi`
--
ALTER TABLE `hesodonvi`
  ADD CONSTRAINT `hesodonvi_ibfk_1` FOREIGN KEY (`MANGUYENLIEU`) REFERENCES `nguyenlieu` (`MANGUYENLIEU`),
  ADD CONSTRAINT `hesodonvi_ibfk_2` FOREIGN KEY (`MADONVI`) REFERENCES `donvi` (`MADONVI`);

--
-- Các ràng buộc cho bảng `hoadon`
--
ALTER TABLE `hoadon`
  ADD CONSTRAINT `FK_HD_BAN` FOREIGN KEY (`MABAN`) REFERENCES `ban` (`MABAN`),
  ADD CONSTRAINT `FK_HD_KH` FOREIGN KEY (`MAKHACHHANG`) REFERENCES `khachhang` (`MAKHACHHANG`),
  ADD CONSTRAINT `FK_HD_NV` FOREIGN KEY (`MANHANVIEN`) REFERENCES `nhanvien` (`MANHANVIEN`),
  ADD CONSTRAINT `FK_HD_TT` FOREIGN KEY (`MATT`) REFERENCES `thanhtoan` (`MATT`);

--
-- Các ràng buộc cho bảng `lichlamviec`
--
ALTER TABLE `lichlamviec`
  ADD CONSTRAINT `FK_LLV_CA` FOREIGN KEY (`MACA`) REFERENCES `calam` (`MACA`),
  ADD CONSTRAINT `FK_LLV_NV` FOREIGN KEY (`MANHANVIEN`) REFERENCES `nhanvien` (`MANHANVIEN`);

--
-- Các ràng buộc cho bảng `loai`
--
ALTER TABLE `loai`
  ADD CONSTRAINT `FK_LOAI_NHOM` FOREIGN KEY (`MANHOM`) REFERENCES `nhom` (`MANHOM`);

--
-- Các ràng buộc cho bảng `nguyenlieu`
--
ALTER TABLE `nguyenlieu`
  ADD CONSTRAINT `nguyenlieu_ibfk_1` FOREIGN KEY (`MADONVICOSO`) REFERENCES `donvi` (`MADONVI`);

--
-- Các ràng buộc cho bảng `phanquyen`
--
ALTER TABLE `phanquyen`
  ADD CONSTRAINT `phanquyen_ibfk_1` FOREIGN KEY (`MAVAITRO`) REFERENCES `vaitro` (`MAVAITRO`) ON DELETE CASCADE,
  ADD CONSTRAINT `phanquyen_ibfk_2` FOREIGN KEY (`MAQUYEN`) REFERENCES `quyen` (`MAQUYEN`) ON DELETE CASCADE;

--
-- Các ràng buộc cho bảng `phieunhap`
--
ALTER TABLE `phieunhap`
  ADD CONSTRAINT `FK_PN_NCC` FOREIGN KEY (`MANCC`) REFERENCES `nhacungcap` (`MANCC`);

--
-- Các ràng buộc cho bảng `sanpham`
--
ALTER TABLE `sanpham`
  ADD CONSTRAINT `FK_SANPHAM_LOAI` FOREIGN KEY (`MALOAI`) REFERENCES `loai` (`MALOAI`);

--
-- Các ràng buộc cho bảng `taikhoan`
--
ALTER TABLE `taikhoan`
  ADD CONSTRAINT `FK_TAIKHOAN_NHANVIEN` FOREIGN KEY (`MANHANVIEN`) REFERENCES `nhanvien` (`MANHANVIEN`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
