package com.Info.InfoApp.service;

import com.Info.InfoApp.Entity.User;
import com.Info.InfoApp.Entity.VerificationToken;
import com.Info.InfoApp.model.UserModel;

import java.io.IOException;

public interface UserService {
    User registerUser(UserModel userModel) throws IOException;
    User findUserByEmail(String email);
    void saveVerificationTokenForUser(String token, User user);
    String validateVerificationToken(String token);
    VerificationToken generateNewVerificationToken(String oldToken);
}
