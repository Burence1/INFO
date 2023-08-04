package com.Info.InfoApp.auth.config;


import com.Info.InfoApp.auth.token.jwtAuthFilter;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.AuthenticationProvider;
import org.springframework.security.authentication.dao.DaoAuthenticationProvider;
import org.springframework.security.config.annotation.authentication.configuration.AuthenticationConfiguration;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.config.http.SessionCreationPolicy;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.security.web.SecurityFilterChain;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.web.authentication.UsernamePasswordAuthenticationFilter;
import org.springframework.security.config.annotation.method.configuration.EnableMethodSecurity;

@Configuration
@EnableWebSecurity
@EnableMethodSecurity
public class WebSecurityConfig {

    @Autowired
    private jwtAuthFilter authFilter;
    public static final String[] WHITE_LIST_URLS =
            {"/register","/resendVerifyToken","/verifyRegistration","/authenticate"};

    @Bean
    public BCryptPasswordEncoder bCryptPasswordEncoder() {
        return new BCryptPasswordEncoder();
    }

    @Bean
    public SecurityFilterChain securityFilterChain(HttpSecurity httpSecurity) throws Exception{
//      httpSecurity
//              .cors(cors ->cors.disable())
//              .csrf(csrf -> csrf.disable())
//              .authorizeHttpRequests(auth -> auth
//                      .requestMatchers(WHITE_LIST_URLS).permitAll()
//                      .anyRequest().authenticated())
//              .authenticationProvider(authenticationProvider())
//              .addFilterBefore(authFilter, UsernamePasswordAuthenticationFilter.class)
//              ;

       return httpSecurity.csrf(csrf -> csrf.disable())
                .cors(cors ->cors.disable())
                .authorizeHttpRequests(requests -> requests
                .requestMatchers(WHITE_LIST_URLS).permitAll()
                .anyRequest().authenticated())
                .sessionManagement(sessionManagement -> sessionManagement.sessionCreationPolicy(SessionCreationPolicy.STATELESS))
                .authenticationProvider(authenticationProvider())
                .addFilterBefore(authFilter, UsernamePasswordAuthenticationFilter.class).build();
    };

    @Bean
    public UserDetailsService userDetailsService() {
        return new UserInfoDetailsService();
    }
    @Bean
    public AuthenticationProvider authenticationProvider(){
        DaoAuthenticationProvider authenticationProvider=new DaoAuthenticationProvider();
        authenticationProvider.setUserDetailsService(userDetailsService());
        authenticationProvider.setPasswordEncoder(bCryptPasswordEncoder());
        return authenticationProvider;
    }
    @Bean
    public AuthenticationManager authenticationManager(AuthenticationConfiguration config) throws Exception {
        return config.getAuthenticationManager();
    }
}