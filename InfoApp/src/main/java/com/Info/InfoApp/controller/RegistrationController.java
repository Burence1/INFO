package com.Info.InfoApp.controller;

import com.Info.InfoApp.Entity.User;
import com.Info.InfoApp.model.UserModel;
import com.Info.InfoApp.service.UserService;
import jakarta.servlet.http.HttpServletRequest;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

import java.io.IOException;
import java.util.Formatter;

@RestController
public class RegistrationController {

    @Autowired
    private UserService userService;

    @PostMapping("/register")
    public String reisterUser(@RequestBody UserModel userModel, final HttpServletRequest request){
        try{
            User user = userService.registerUser(userModel);
            return new Formatter().format("Successfully Created %s", user.getFirstName() + " " +
                    user.getLastName()).toString();
        }
        catch (Exception e){
            return e.getMessage();
        }

    }
}
