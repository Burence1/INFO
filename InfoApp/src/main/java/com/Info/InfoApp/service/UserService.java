package com.Info.InfoApp.service;

import com.Info.InfoApp.Entity.User;
import com.Info.InfoApp.model.UserModel;

import java.io.IOException;

public interface UserService {
    User registerUser(UserModel userModel) throws IOException;

    User findUserByEmail(String email);

}
