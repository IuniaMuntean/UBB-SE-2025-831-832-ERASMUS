-- Create main schema
CREATE SCHEMA transport;

-- Drivers table
CREATE TABLE transport.drivers (
    driver_id SERIAL PRIMARY KEY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    license_number VARCHAR(20) UNIQUE NOT NULL,
    phone VARCHAR(20),
    email VARCHAR(100),
    hire_date DATE NOT NULL,
    status VARCHAR(15) NOT NULL CHECK (status IN ('ACTIVE', 'INACTIVE', 'VACATION', 'SICK_LEAVE'))
);

-- Trucks table
CREATE TABLE transport.trucks (
    truck_id SERIAL PRIMARY KEY,
    license_plate VARCHAR(15) UNIQUE NOT NULL,
    make VARCHAR(30) NOT NULL,
    model VARCHAR(30) NOT NULL,
    year INTEGER NOT NULL,
    capacity_kg DECIMAL(10, 2) NOT NULL,
    status VARCHAR(15) NOT NULL CHECK (status IN ('AVAILABLE', 'IN_MAINTENANCE', 'IN_USE', 'RETIRED')),
    last_maintenance_date DATE,
    next_maintenance_date DATE
);

-- Cities table
CREATE TABLE transport.cities
(
    id INT PRIMARY KEY,
    name VARCHAR(255),
    x FLOAT,
    y FLOAT    
);

-- Roads table
CREATE TABLE transport.roads
(
    id int,
    startcityid INT,
    endcityid INT,
    distance FLOAT,
    FOREIGN KEY (startcityid) REFERENCES transport.cities(id),
    FOREIGN KEY (endcityid) REFERENCES transport.cities(id),
    PRIMARY KEY (startcityid, endcityid)
);

-- Orders table
CREATE TABLE transport.orders (
    order_id SERIAL PRIMARY KEY,
    client_name VARCHAR(255) NOT NULL,
    cargo_type VARCHAR(255) NOT NULL,
    cargo_weight DOUBLE PRECISION NOT NULL,
    source_city_id INTEGER NOT NULL,
    destination_city_id INTEGER NOT NULL,
    FOREIGN KEY (source_city_id) REFERENCES transport.cities(id),
    FOREIGN KEY (destination_city_id) REFERENCES transport.cities(id)
);

-- Deliveries table
CREATE TABLE transport.deliveries (
    delivery_id SERIAL PRIMARY KEY,
    order_id INTEGER NOT NULL,
    driver_id INTEGER,
    truck_id INTEGER,
    status VARCHAR(20) NOT NULL CHECK (status IN ('PLANNED', 'IN_PROGRESS', 'COMPLETED', 'CANCELLED')),
    departure_time TIMESTAMP WITH TIME ZONE NOT NULL,
    estimated_time_arrival TIMESTAMP WITH TIME ZONE NOT NULL,
    total_distance_km DECIMAL(10, 2) NOT NULL,
    fee_euros DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (order_id) REFERENCES transport.orders(order_id),
    FOREIGN KEY (driver_id) REFERENCES transport.drivers(driver_id),
    FOREIGN KEY (truck_id) REFERENCES transport.trucks(truck_id)
);

-- Users table
CREATE TABLE transport.users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(255) NOT NULL,
    password VARCHAR(255) NOT NULL
);

CREATE TABLE transport.roadfinancials
(
    startcityid INT,
    endcityid INT,
    cost FLOAT,
    revenue FLOAT,
    PRIMARY KEY (startcityid, endcityid),
    FOREIGN KEY (startcityid, endcityid)
        REFERENCES transport.roads(startcityid, endcityid)
);
