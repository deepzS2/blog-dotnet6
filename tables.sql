CREATE TABLE IF NOT EXISTS `users` (
  id INT NOT NULL AUTO_INCREMENT, 
  username VARCHAR(255), 
  email varchar(255), 
  password VARCHAR(255), 
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP, 
  updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP, 
  UNIQUE (id)
);