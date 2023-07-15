package com.Info.InfoApp.models;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import lombok.*;
import org.springframework.security.core.userdetails.UserDetails;

@Entity
@Data
@NoArgsConstructor
public class User {

    @Id
    @GeneratedValue(
            strategy = GenerationType.AUTO
    )
    private long Id;
    private String UserName;
    private String email;
    private String password;

    public User(String userName,String email,String password){
        this.UserName = userName;
        this.email = email;
        this.password = password;
    }
}
