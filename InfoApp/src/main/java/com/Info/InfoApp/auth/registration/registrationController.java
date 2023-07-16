package com.Info.InfoApp.auth.registration;

import com.Info.InfoApp.models.RegistrationRequest;
import com.Info.InfoApp.services.userService;
import lombok.AllArgsConstructor;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping(path = "api/v1/registration")
@AllArgsConstructor
public class registrationController {

    private final registrationService registrationService;

    @PostMapping
    private String Register(@RequestBody RegistrationRequest request){
        return registrationService.register(request);
    }

    @GetMapping(path = "confirm")
    public String confirm(@RequestParam("token") String token) {
        return registrationService.confirmToken(token);
    }
}
