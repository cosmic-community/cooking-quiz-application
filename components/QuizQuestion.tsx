'use client'

import { useState } from 'react'
import type { Question } from '@/types'

interface QuizQuestionProps {
  question: Question
  questionNumber: number
  onAnswer: (selectedOption: number) => void
  disabled?: boolean
}

export default function QuizQuestion({ 
  question, 
  questionNumber, 
  onAnswer,
  disabled = false 
}: QuizQuestionProps) {
  const [selectedOption, setSelectedOption] = useState<number | null>(null)
  const [showFeedback, setShowFeedback] = useState(false)

  const handleOptionClick = (index: number) => {
    if (disabled || showFeedback) return
    
    setSelectedOption(index)
    setShowFeedback(true)
    
    setTimeout(() => {
      onAnswer(index)
      setSelectedOption(null)
      setShowFeedback(false)
    }, 1500)
  }

  const getOptionStyle = (index: number) => {
    if (!showFeedback) {
      return selectedOption === index ? 'question-option selected' : 'question-option'
    }
    
    if (index === question.metadata.correct_answer) {
      return 'question-option correct'
    }
    
    if (selectedOption === index) {
      return 'question-option incorrect'
    }
    
    return 'question-option'
  }

  return (
    <div className="animate-slide-in">
      <h2 className="text-2xl font-semibold mb-6">
        Question {questionNumber}: {question.metadata.question_text}
      </h2>
      
      <div className="space-y-3">
        {question.metadata.options.map((option, index) => (
          <button
            key={option.id}
            onClick={() => handleOptionClick(index)}
            className={getOptionStyle(index)}
            disabled={disabled || showFeedback}
          >
            <span className="font-medium mr-3">
              {String.fromCharCode(65 + index)}.
            </span>
            {option.text}
          </button>
        ))}
      </div>
      
      {showFeedback && question.metadata.explanation && (
        <div className="mt-4 p-4 bg-blue-50 border-2 border-blue-200 rounded-lg">
          <p className="text-sm text-gray-700">
            <strong>Explanation:</strong> {question.metadata.explanation}
          </p>
        </div>
      )}
      
      {disabled && (
        <div className="mt-4 p-4 bg-yellow-50 border-2 border-yellow-200 rounded-lg">
          <p className="text-sm text-yellow-800">
            Time's up! Moving to results...
          </p>
        </div>
      )}
    </div>
  )
}