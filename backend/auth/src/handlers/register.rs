use axum::{Json, extract::State};
use argon2::{
    Argon2, PasswordHasher,
    password_hash::{SaltString, rand_core::OsRng},
};

use crate::{db::DbPool, models::{RegisterRequest, UserResponse}, error::AppError, db::create_user};

pub async fn register(
    State(pool): State<DbPool>,
    Json(req): Json<RegisterRequest>,
) -> Result<Json<UserResponse>, AppError> {
    let salt = SaltString::generate(&mut OsRng);
    let argon2 = Argon2::default();
    let password_hash = argon2
        .hash_password(req.password.as_bytes(), &salt)
        .map_err(|_| AppError::InvalidCredentials)?
        .to_string();
    
    let user = create_user(&pool, &req.email, &password_hash)?;
    
    Ok(Json(UserResponse {
        id: user.id,
        email: user.email,
    }))
}