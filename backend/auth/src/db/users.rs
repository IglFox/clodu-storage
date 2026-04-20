use super::DbPool;
use crate::models::user::User;  // Убрали CreateUser
use crate::error::AppError;
use rusqlite::{params, OptionalExtension};
use uuid::Uuid;
use chrono::Utc;

pub fn create_user(pool: &DbPool, email: &str, password_hash: &str) -> Result<User, AppError> {
    let conn = pool.lock().unwrap();
    
    let id = Uuid::new_v4().to_string();
    let now = Utc::now().to_rfc3339();
    
    let result = conn.execute(
        "INSERT INTO users (id, email, password_hash, created_at, updated_at) 
         VALUES (?1, ?2, ?3, ?4, ?5)",
        params![&id, email, password_hash, &now, &now],
    );
    
    match result {
        Ok(_) => Ok(User {
            id,
            email: email.to_string(),
            created_at: now.clone(),
            updated_at: now,
        }),
        Err(e) => {
            if e.to_string().contains("UNIQUE constraint failed") {
                Err(AppError::UserAlreadyExists)
            } else {
                Err(AppError::Database(e))
            }
        }
    }
}

pub fn find_user_by_email(pool: &DbPool, email: &str) -> Result<Option<(User, String)>, AppError> {
    let conn = pool.lock().unwrap();
    
    let result = conn.query_row(
        "SELECT id, email, password_hash, created_at, updated_at FROM users WHERE email = ?1",
        params![email],
        |row| {
            Ok((
                User {
                    id: row.get(0)?,
                    email: row.get(1)?,
                    created_at: row.get(3)?,  // индексы: 0-id, 1-email, 2-password_hash, 3-created_at, 4-updated_at
                    updated_at: row.get(4)?,
                },
                row.get::<_, String>(2)?,
            ))
        },
    ).optional()?;
    
    Ok(result)
}