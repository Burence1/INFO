package com.Info.InfoApp.auth.config;

import com.Info.InfoApp.Entity.User;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import java.util.Arrays;
import java.util.Collection;
import java.util.List;
import java.util.stream.Collectors;
import java.util.stream.Stream;

public class UserInfoDetails implements UserDetails {

    private String userName;
    private String password;
    private List<GrantedAuthority> roles;

    public UserInfoDetails(User user) {
        this.userName = user.getUserName();
        this.password = user.getPassword();
        String rolesString = user.getRoles();

        this.roles = Stream.ofNullable(rolesString)
                .map(s -> s.split(","))
                .flatMap(Arrays::stream)
                .map(SimpleGrantedAuthority::new)
                .collect(Collectors.toList());
//        this.roles = Arrays.stream(user.getRoles().split(","))
//                .map(SimpleGrantedAuthority::new)
//                .collect(Collectors.toList());
    }

    @Override
    public Collection<? extends GrantedAuthority> getAuthorities() {
        return roles;
    }

    @Override
    public String getPassword() {
        return password;
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
        return true;
    }

    @Override
    public boolean isCredentialsNonExpired() {
        return true;
    }

    @Override
    public boolean isEnabled() {
        return true;
    }
}
