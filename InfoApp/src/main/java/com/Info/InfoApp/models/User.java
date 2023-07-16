package com.Info.InfoApp.models;

import com.Info.InfoApp.enums.UserRole;
import jakarta.persistence.*;
import lombok.*;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.authority.SimpleGrantedAuthority;

import java.math.BigInteger;
import java.util.Collections;
import java.util.Collection;

@Entity
@Data
@NoArgsConstructor
@Table(name ="app_user")
public class User implements UserDetails{

    @Id
    @GeneratedValue(
            strategy = GenerationType.IDENTITY
    )
    @Column(name = "user_id", nullable = false, columnDefinition = "NUMERIC(38,0)", unique = true)
    private BigInteger Id;
    private String UserName;
    private String email;
    private String password;
    private Boolean enabled = false;
    @Enumerated(EnumType.STRING)
    private UserRole userRole;
    private Boolean locked = false;

    public User(String userName,String email,String password){
        this.UserName = userName;
        this.email = email;
        this.password = password;
    }

    @Override
    public Collection<? extends GrantedAuthority> getAuthorities() {
        SimpleGrantedAuthority authority =
                new SimpleGrantedAuthority(userRole.name());
        return Collections.singletonList(authority);
    }

    @Override
    public String getUsername() {
        return UserName;
    }

    @Override
    public boolean isAccountNonExpired() {
        return true;
    }

    @Override
    public boolean isAccountNonLocked() {
        return !locked;
    }

    @Override
    public boolean isCredentialsNonExpired() {
        return true;
    }

    @Override
    public boolean isEnabled() {
        return enabled;
    }
}
