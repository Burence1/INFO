package com.Info.InfoApp.Entity;

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
@Table(name ="appuser")
public class User implements UserDetails{

    @Id
    @GeneratedValue(
            strategy = GenerationType.IDENTITY
    )
    @Column(name = "user_id", nullable = false, columnDefinition = "NUMERIC(38,0)", unique = true)
    private BigInteger Id;
    private String userName;
    private String email;
    private String password;
    private Boolean enabled = false;
    @Enumerated(EnumType.STRING)
    private UserRole userRole;
    private Boolean locked = false;

    public User(String userName,String email,String password,UserRole userRole){
        this.userName = userName;
        this.email = email;
        this.password = password;
        this.userRole = userRole;
    }

    @Override
    public Collection<? extends GrantedAuthority> getAuthorities() {
        SimpleGrantedAuthority authority =
                new SimpleGrantedAuthority(userRole.name());
        return Collections.singletonList(authority);
    }

    @Override
    public String getUsername() {
        return userName;
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