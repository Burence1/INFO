package com.Info.InfoApp.service;

import io.jsonwebtoken.Claims;
import org.springframework.security.core.userdetails.UserDetails;

import java.security.Key;
import java.util.Date;
import java.util.Map;
import java.util.function.Function;

public interface JwtService {

    String createToken(Map<String, Object> claims, String username);
    String generateToken(String username);
    Key getSignKey();
    String extractUserName(String token);

    <T> T extractClaim(String token, Function<Claims, T> claimsResolver);
    Claims extractAllClaims(String token);
    Date extractExpiration(String token);
    Boolean validateToken(String token, UserDetails userDetails);

    Boolean isTokenExpired(String token);
}
