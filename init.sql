USE demo_db;

CREATE TABLE customers (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100),
    email VARCHAR(100),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE orders (
    id INT AUTO_INCREMENT PRIMARY KEY,
    customer_id INT,
    amount DECIMAL(10,2),
    order_date DATE,
    FOREIGN KEY (customer_id) REFERENCES customers(id)
);

INSERT INTO customers (name, email) VALUES
('Alice Martin', 'alice@example.com'),
('Bob Dupont', 'bob@example.com'),
('Charlie Durand', 'charlie@example.com');

INSERT INTO orders (customer_id, amount, order_date) VALUES
(1, 120.50, '2025-10-01'),
(1, 49.99, '2025-10-03'),
(2, 75.00, '2025-10-05'),
(3, 210.00, '2025-10-07');