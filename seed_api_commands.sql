-- Seed ApiCommands table for Notification API (same values as SSCGI)
-- Copy the actual values from SSCGI_Website database: SELECT * FROM ApiCommands WHERE IsActive = 1
-- Replace the placeholder values below with SSCGI's actual values

INSERT INTO ApiCommands (Code, Name, Value, IsActive) VALUES ('NotificationApi', 'BaseUrl', '<<COPY FROM SSCGI ApiCommands>>', 1);
INSERT INTO ApiCommands (Code, Name, Value, IsActive) VALUES ('NotificationApi', 'ClientId', '<<COPY FROM SSCGI ApiCommands>>', 1);
INSERT INTO ApiCommands (Code, Name, Value, IsActive) VALUES ('NotificationApi', 'ClientKey', '<<COPY FROM SSCGI ApiCommands>>', 1);
INSERT INTO ApiCommands (Code, Name, Value, IsActive) VALUES ('NotificationApi', 'Token', '', 1);
INSERT INTO ApiCommands (Code, Name, Value, IsActive) VALUES ('NotificationApi', 'Expired', '2000-01-01', 1);
