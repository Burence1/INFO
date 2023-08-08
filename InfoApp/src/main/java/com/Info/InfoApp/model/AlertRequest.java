package com.Info.InfoApp.model;

import jakarta.persistence.Column;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import org.hibernate.annotations.CreationTimestamp;

import java.time.LocalDateTime;

@Data
@NoArgsConstructor
@AllArgsConstructor
public class AlertRequest {
    private String alertNo;
    private String alertName;
    private String alertSubject;
    private String messageBody;
    private String captureUser;
    private boolean isEnabled;
    private LocalDateTime creationDate;
}
