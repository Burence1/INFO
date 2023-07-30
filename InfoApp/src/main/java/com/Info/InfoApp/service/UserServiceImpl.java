package com.Info.InfoApp.service;

import com.Info.InfoApp.Entity.User;
import com.Info.InfoApp.model.UserModel;
import com.Info.InfoApp.repository.UserRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.stereotype.Service;

import java.io.IOException;
import java.util.InputMismatchException;
import java.util.regex.*;

@Service
public class UserServiceImpl implements UserService{

    @Autowired
    private UserRepository userRepository;

    @Autowired
    private BCryptPasswordEncoder passwordEncoder;

    String regex = "^(.+)@(.+)$";
    Pattern pattern = Pattern.compile(regex);

    @Override
    public User registerUser(UserModel userModel) throws IOException {
        User user = new User();
        Matcher matcher = pattern.matcher(userModel.getEmail());
        var match = matcher.matches();

        if(!match)
            throw new IOException("sorry device error");

        user.setEmail(userModel.getEmail());
        user.setFirstName(userModel.getFirstName());
        user.setLastName(user.getLastName());
        user.setRole("USER");
        user.setPassword(passwordEncoder.encode(userModel.getPassword()));

        userRepository.save(user);
        return user;
    }

    @Override
    public User findUserByEmail(String email) {
        return userRepository.findByEmail(email);
    }
}