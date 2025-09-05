'use client'

import { useEffect, useState } from 'react'
import { Clock } from 'lucide-react'

interface TimerProps {
  initialTime: number // in seconds
  onTimeUp: () => void
}

export default function Timer({ initialTime, onTimeUp }: TimerProps) {
  const [timeRemaining, setTimeRemaining] = useState(initialTime)

  useEffect(() => {
    if (timeRemaining <= 0) {
      onTimeUp()
      return
    }

    const timer = setInterval(() => {
      setTimeRemaining((prev) => {
        if (prev <= 1) {
          clearInterval(timer)
          return 0
        }
        return prev - 1
      })
    }, 1000)

    return () => clearInterval(timer)
  }, [timeRemaining, onTimeUp])

  const formatTime = (seconds: number) => {
    const mins = Math.floor(seconds / 60)
    const secs = seconds % 60
    return `${mins.toString().padStart(2, '0')}:${secs.toString().padStart(2, '0')}`
  }

  const getTimerColor = () => {
    const percentageRemaining = (timeRemaining / initialTime) * 100
    if (percentageRemaining <= 10) return 'text-error timer-warning'
    if (percentageRemaining <= 25) return 'text-warning'
    return 'text-gray-700'
  }

  return (
    <div className={`flex items-center gap-2 font-semibold ${getTimerColor()}`}>
      <Clock className="w-5 h-5" />
      <span>{formatTime(timeRemaining)}</span>
    </div>
  )
}