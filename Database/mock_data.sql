-- Insert drivers
INSERT INTO transport.drivers (first_name, last_name, license_number, phone, email, hire_date, status)
VALUES
    ('Juan', 'Pérez', 'LIC-123456', '+34611223344', 'juan.perez@transportesrapidos.com', '2020-01-15', 'ACTIVE'),
    ('María', 'Gómez', 'LIC-234567', '+34622334455', 'maria.gomez@transportesrapidos.com', '2019-05-22', 'ACTIVE'),
    ('Carlos', 'López', 'LIC-345678', '+34633445566', 'carlos.lopez@enviosseguros.com', '2021-03-10', 'VACATION'),
    ('Ana', 'Martínez', 'LIC-456789', '+34644556677', 'ana.martinez@cargaeficiente.com', '2022-01-30', 'ACTIVE'),
    ('Pedro', 'Sánchez', 'LIC-567890', '+34655667788', 'pedro.sanchez@transportesrapidos.com', '2018-11-05', 'INACTIVE');

-- Insert trucks
INSERT INTO transport.trucks (license_plate, make, model, year, capacity_kg, status, last_maintenance_date, next_maintenance_date)
VALUES
    ('1234ABC', 'Volvo', 'FH16', 2020, 40000.00, 'AVAILABLE', '2023-01-15', '2023-07-15'),
    ('2345BCD', 'MAN', 'TGX', 2019, 35000.00, 'IN_USE', '2023-02-20', '2023-08-20'),
    ('3456CDE', 'Scania', 'R450', 2021, 42000.00, 'AVAILABLE', '2023-03-10', '2023-09-10'),
    ('4567DEF', 'Mercedes-Benz', 'Actros', 2018, 38000.00, 'IN_MAINTENANCE', '2022-12-05', '2023-06-05'),
    ('5678EFG', 'Iveco', 'S-WAY', 2022, 39000.00, 'AVAILABLE', '2023-04-01', '2023-10-01');

-- Insert Cities
INSERT INTO transport.cities(id, name, x, y)
VALUES
    (1, 'Bucharest', 210, 451),
    (2, 'Cluj-Napoca', 124, 149),
    (3, 'Iasi', 297, 85),
    (4, 'Constanta', 500, 484),
    (5, 'Timisoara', 0, 264),
    (6, 'Brasov', 161, 194),
    (7, 'Craiova', 78, 435),
    (8, 'Galati', 436, 277),
    (9, 'Oradea', 66, 0),
    (10, 'Ploiesti', 203, 384);

-- Insert Roads
INSERT INTO transport.roads(id, startCityID, endCityID, distance)
VALUES
    (1, 1, 2, 3.6),
    (2, 1, 3, 3.2),
    (3, 1, 4, 2.7),
    (4, 1, 5, 4.6),
    (5, 1, 6, 1.8),
    (6, 1, 7, 1.2),
    (7, 1, 8, 2.1),
    (8, 1, 9, 4.7),
    (9, 1, 10,6.2),
    (10, 2, 3, 3.1),
    (11, 2, 4, 3.5),
    (12, 2, 5, 1.8),
    (13, 2, 6, 1.9),
    (14, 2, 7, 2.3),
    (15, 2, 8, 2.4),
    (16, 2, 9, 1.1),
    (17, 2, 10,2.5),
    (18, 3, 4, 3.4),
    (19, 3, 5, 2.2),
    (20, 3, 6, 2.6),
    (21, 3, 7, 3.7),
    (22, 3, 8, 9.3),
    (23, 3, 9, 3.7),
    (24, 3, 10,3.1),
    (25, 4, 5, 4.8),
    (26, 4, 6, 2.9),
    (27, 4, 7, 3.9),
    (28, 4, 8, 9.5),
    (29, 4, 9, 3.9),
    (30, 4, 10,3.3);

-- Insert Orders
INSERT INTO transport.orders (client_name, cargo_type, cargo_weight, source_city_id, destination_city_id)
VALUES
    ('Electronics Corp', 'Electronics', 12500.50, 1, 2),
    ('Auto Parts Ltd', 'Auto Parts', 18500.75, 2, 3),
    ('Office Supplies Inc', 'Office Furniture', 22000.00, 3, 4),
    ('Construction Co', 'Building Materials', 30000.25, 4, 5),
    ('Food Distributors', 'Food Products', 15000.00, 5, 6),
    ('Medical Supplies Co', 'Medical Equipment', 8500.00, 6, 7),
    ('Textile Industries', 'Textiles', 25000.00, 7, 8),
    ('Chemical Solutions', 'Chemicals', 18000.50, 8, 9),
    ('Agricultural Products', 'Agricultural Goods', 22000.75, 9, 10),
    ('Retail Chain', 'Consumer Goods', 12000.25, 10, 1);

-- Insert Deliveries
INSERT INTO transport.deliveries (
    order_id, driver_id, truck_id, status,
    departure_time, estimated_time_arrival,
    total_distance_km, fee_euros
)
VALUES
    (1, 1, 1, 'COMPLETED',
     '2023-06-01 08:00:00+02', '2023-06-01 14:00:00+02',
     620.5, 850.00),
     
    (2, 2, 2, 'IN_PROGRESS',
     '2023-06-02 09:30:00+02', '2023-06-02 12:30:00+02',
     320.0, 600.50),
     
    (3, 4, 3, 'PLANNED',
     '2023-06-03 07:00:00+02', '2023-06-03 18:00:00+02',
     850.0, 1200.00),
     
    (4, 1, 5, 'COMPLETED',
     '2023-06-01 10:00:00+02', '2023-06-01 20:00:00+02',
     720.0, 950.75),
     
    (5, 4, 1, 'PLANNED',
     '2023-06-04 06:00:00+02', '2023-06-04 12:00:00+02',
     550.0, 700.00),

    (6, 3, 2, 'IN_PROGRESS',
     '2023-06-05 07:30:00+02', '2023-06-05 11:30:00+02',
     280.0, 450.00),

    (7, 2, 3, 'PLANNED',
     '2023-06-06 08:00:00+02', '2023-06-06 16:00:00+02',
     680.0, 900.00),

    (8, 1, 4, 'CANCELLED',
     '2023-06-07 09:00:00+02', '2023-06-07 15:00:00+02',
     420.0, 650.00),

    (9, 4, 5, 'PLANNED',
     '2023-06-08 06:30:00+02', '2023-06-08 13:30:00+02',
     580.0, 780.00),

    (10, 3, 1, 'IN_PROGRESS',
     '2023-06-09 07:00:00+02', '2023-06-09 12:00:00+02',
     350.0, 550.00);
